//using GlassCockpit.Model;
//using GlassCockpit.Utility;
using FSComm.Model;
using FSComm.Utility;
using FSComm.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace FSComm.ViewModel
{
    public class SimpleHTTPServerViewModel : HttpServer
    {
        private MainWindowViewModel MainWindowViewModel { get; }
        private DatabaseViewModel DatabaseViewModel { get { return this.MainWindowViewModel.DatabaseViewModel; } }

        public WebSocketServiceHost SimChannel { get; }
        internal HtmlFolders HtmlFolders { get; }
        public string FolderFS { get; }
        public string FolderOfficial { get; }
        public string FolderCommunity { get; }

        public SimpleHTTPServerViewModel(MainWindowViewModel mainwindowviewmodel, int port, string document_root) : base(System.Net.IPAddress.Any, port)
        {
            this.MainWindowViewModel = mainwindowviewmodel;
            this.DocumentRootPath = document_root;
            this.OnGet += SimpleHttpServer_OnGet;
            this.MainWindowViewModel.LogViewModel.Add($"\nHTTPServer: {System.Net.IPAddress.Any}:{port}");


            this.AddWebSocketService<SimChannel>("/SimChannel");
            this.SimChannel = this.WebSocketServices["/SimChannel"];
            
            this.HtmlFolders = new HtmlFolders();
            this.HtmlFolders.FindDirectories(MainWindowViewModel.AppSettingsViewModel.FSContentsFolder);
            this.HtmlFolders.FindDirectories(MainWindowViewModel.AppSettingsViewModel.FSFolder);
            //this.FolderFS = @"C:\MSFS2020";
            this.FolderCommunity = MainWindowViewModel.AppSettingsViewModel.FSContentsFolder + @"\Community";
            this.HtmlFolders.Add(this.FolderCommunity);
            this.FolderOfficial = MainWindowViewModel.AppSettingsViewModel.FSContentsFolder + @"\Official\OneStore";
            //this.HtmlFolders.FindDirectories(this.FolderFS);
            this.HtmlFolders.Add(this.FolderOfficial);
            ////this.HtmlFolders.FindDirectories(@"C:\MSFS2020" + @"\Official\OneStore");
            //this.HtmlFolders.FindDirectories(@"C:\Program Files\WindowsApps\Microsoft.FlightSimulator_1.27.13.0_x64__8wekyb3d8bbwe\Packages\fs-base-ui");
            foreach (string d in this.HtmlFolders)
            {
                this.MainWindowViewModel.LogViewModel.Add(d);
            }
        }

        public void SendSerialData(string data)
        {
            //this.MainWindowViewModel.LogViewModel.Add($"MQTT Pupblish\r\n\tTopic: serial/data\r\n\tPayload: {data}\n");
            //await this.Client.PublishAsync("serial/data", data);
            var msg = $"{{\"clientId\": \"SERVER\", \"topic\": \"serial/data\", \"payload\": \"{data}\", \"timestamp\": 0}}";
            this.SimChannel.Sessions.Broadcast(msg);
        }

        private void SimpleHttpServer_OnGet(object sender, HttpRequestEventArgs e)
        {
            var req = e.Request;
            var res = e.Response;
            var path = req.RawUrl;
            string log = "----------------------------------------------------------------------------------\r\n";

            try
            {
                res.AppendHeader("access-control-allow-origin", "*");

                byte[] contents;

                if (path.StartsWith("/query?"))
                {
                    string resp = "";
                    if (this.DatabaseViewModel != null)
                    {
                        string q = e.Request.QueryString.Get("q");
                        if (q == "airport")
                        //if (q == "getairportinfo")
                        {
                            string airport = e.Request.QueryString.Get("airport").ToUpper();

                            List<AirPort> airports = this.DatabaseViewModel.FillAirports(airport);

                            resp = JsonConvert.SerializeObject(airports);


                            contents = System.Text.Encoding.UTF8.GetBytes(resp);
                            res.ContentType = MimeTypes.Mappings[".json"];
                            res.AddHeader("Access-Control-Allow-Origin", "*");
                            res.ContentLength64 = contents.LongLength;
                            res.Close(contents, true);
                            return;
                        }
                        else if(q == "airportdata")
                        {
                            string airport = e.Request.QueryString.Get("airport").ToUpper();

                            AirPort ai = this.DatabaseViewModel.FillAirports(airport).FirstOrDefault();
                            if (ai != null)
                            {

                                //ai.Runways = this.DatabaseViewModel.Runways.Where(k => k.airport_id == ai.airport_id).ToList();
                                ai.Runways = this.DatabaseViewModel.FillRunways(ai.airport_id).ToList();
                                //ai.Approaches = this.DatabaseViewModel.Approaches.Where(k => k.airport_id == ai.airport_id).OrderBy(k => k.runway_name).OrderBy(k => k.fix_type).ToList();
                                ai.Approaches = this.DatabaseViewModel.FillApproaches(ai.airport_id).ToList();
                                foreach (Approach apr in ai.Approaches)
                                {
                                    //apr.Legs = this.DatabaseViewModel.ApproachLegs.Where(k => k.approach_id == apr.approach_id).OrderBy(k => k.approach_leg_id).OrderByDescending(k => k.approach_leg_id).ToList();
                                    apr.Legs = this.DatabaseViewModel.FillApproachLegs(apr.approach_id).OrderBy(k => k.approach_leg_id).OrderByDescending(k => k.approach_leg_id).ToList();

                                    foreach (ApproachLeg leg in apr.Legs)
                                    {
                                        //Waypoint wp = this.DatabaseViewModel.Waypoints.Where(k => k.ident == leg.fix_ident && k.region == leg.fix_region).FirstOrDefault();
                                        Waypoint wp = this.DatabaseViewModel.FillWaypoints(leg.fix_ident,leg.fix_region).FirstOrDefault();
                                        if (wp == null)
                                        {
                                            //NavSearch ns = this.DatabaseViewModel.NavSearch.Where(k => k.ident == leg.fix_ident && k.region == leg.fix_region).FirstOrDefault();
                                            NavSearch ns = this.DatabaseViewModel.FillNavSearch(leg.fix_ident, leg.fix_region).FirstOrDefault();
                                            if (ns != null)
                                            {
                                                leg.fix_lonx = ns.lonx;
                                                leg.fix_laty = ns.laty;
                                            }
                                        }
                                        else
                                        {
                                            leg.fix_lonx = wp.lonx;
                                            leg.fix_laty = wp.laty;
                                        }
                                    }

                                    //apr.Transitions = this.DatabaseViewModel.Transitions.Where(k => k.approach_id == 46029).ToList();
                                    //apr.Transitions = this.DatabaseViewModel.Transitions.Where(k => k.approach_id == apr.approach_id).OrderBy(k => k.transition_id).ToList();
                                    apr.Transitions = this.DatabaseViewModel.FillTransitions(apr.approach_id).OrderBy(k => k.transition_id).ToList();

                                    foreach (Transition tr in apr.Transitions)
                                    {
                                        //tr.Legs = this.DatabaseViewModel.TransitionLegs.Where(k => k.transition_id == tr.transition_id).OrderByDescending(k => k.transition_leg_id).ToList();
                                        tr.Legs = this.DatabaseViewModel.FillTransitionLegs(tr.transition_id).OrderByDescending(k => k.transition_leg_id).ToList();
                                        foreach (TransitionLeg leg in tr.Legs)
                                        {

                                            Waypoint wp = this.DatabaseViewModel.FillWaypoints(leg.fix_ident, leg.fix_region).FirstOrDefault();
                                            if (wp == null)
                                            {
                                                NavSearch ns = this.DatabaseViewModel.FillNavSearch(leg.fix_ident, leg.fix_region).FirstOrDefault();
                                                if (ns != null)
                                                {
                                                    leg.fix_lonx = ns.lonx;
                                                    leg.fix_laty = ns.laty;
                                                }
                                            }
                                            else
                                            {
                                                leg.fix_lonx = wp.lonx;
                                                leg.fix_laty = wp.laty;
                                            }


                                        }
                                    }

                                }

                                foreach (Runway rw in ai.Runways)
                                {
                                    //rw.Primary = this.DatabaseViewModel.RunwayEnds.Where(k => k.runway_end_id == rw.primary_end_id).FirstOrDefault();
                                    rw.Primary = this.DatabaseViewModel.FillRunwayEnds(rw.primary_end_id).FirstOrDefault();

                                    //rw.Primary.ILS = this.DatabaseViewModel.ILSs.Where(k => k.ident == rw.Primary.ils_ident).FirstOrDefault(); //.ToList();
                                    rw.Primary.ILS = this.DatabaseViewModel.FillILSs(rw.Primary.ils_ident).FirstOrDefault(); //.ToList();

                                    rw.Secondary = this.DatabaseViewModel.FillRunwayEnds(rw.secondary_end_id).FirstOrDefault();
                                    rw.Secondary.ILS = this.DatabaseViewModel.FillILSs(rw.Secondary.ils_ident).FirstOrDefault();
                                }


                                resp = JsonConvert.SerializeObject(ai);
                            }

                            else
                            {
                                resp = "{\"res\": \"NOK\", \"data\": \"NOT FOUND!\"}";
                            }

                        }
                        else if (q == "getairports")
                        {
                            var k = e.Request.QueryString.Get("data");
                            MapDetailsModel cm = JsonConvert.DeserializeObject<MapDetailsModel>(k);
                            Console.WriteLine(cm);

                            //this.DatabaseViewModel.SelectAirPorts(cm);
                            AirPortsInfo ai = new AirPortsInfo();
                            ai.AirPorts = this.DatabaseViewModel.SelectAirPorts(cm);
                            resp = JsonConvert.SerializeObject(ai);

                        }
                        else if (q == "getndbs")
                        {
                            var k = e.Request.QueryString.Get("data");
                            MapDetailsModel cm = JsonConvert.DeserializeObject<MapDetailsModel>(k);
                            Console.WriteLine(cm);

                            //this.DatabaseViewModel.SelectAirPorts(cm);
                            NDBInfo ndb = new NDBInfo();
                            ndb.NDBs = this.DatabaseViewModel.SelectNDBs(cm);
                            resp = JsonConvert.SerializeObject(ndb);

                        }
                        else if (q == "getvors")
                        {
                            var k = e.Request.QueryString.Get("data");
                            MapDetailsModel cm = JsonConvert.DeserializeObject<MapDetailsModel>(k);
                            Console.WriteLine(cm);

                            //this.DatabaseViewModel.SelectAirPorts(cm);
                            VORInfo vi = new VORInfo();
                            vi.VORs = this.DatabaseViewModel.SelectVORs(cm);
                            resp = JsonConvert.SerializeObject(vi);

                        }
                        else
                        {
                            resp = "{\"res\": \"NOK\", \"data\": \"Unknown query!\"}";
                        }
                    }
                    else
                    {
                        resp = "{\"res\": \"NOK\", \"data\": \"DataBaseViewModel is NULL!\"}";
                    }

                    contents = System.Text.Encoding.UTF8.GetBytes(resp);
                    res.ContentType = MimeTypes.Mappings[".json"];
                    res.AddHeader("Access-Control-Allow-Origin", "*");
                    res.ContentLength64 = contents.LongLength;
                    res.Close(contents, true);
                    return;

                }

                if (path == "/")
                    path += "index.html";

                if(path.EndsWith(".map"))
                {
                    res.StatusCode = (int)HttpStatusCode.NotFound;
                    return;
                }

                if(path.Contains("simplane.js"))
                {
                    Console.WriteLine("");
                }

                string localPath = e.Request.Url.LocalPath.Replace('/', '\\');

                if (path.StartsWith("/html_ui/"))
                {
                    path = path.Replace("/html_ui", "");
                    localPath = localPath.Replace("\\html_ui", "");
                    log += $"HTTP REQUEST starts with '/html_ui' => Removed!\r\n";
                }

                string f = this.DocumentRootPath + localPath;


                if (e.TryReadFile(path, out contents))
                {


                    res.ContentType = MimeTypes.Mappings[Path.GetExtension(path)];
                    res.ContentLength64 = contents.LongLength;
                    res.Close(contents, true);
                    //log += $"HTTP OK: {f}\r\n";
                    //write2log = false;
                    return;

                }
                else
                {
                   // log += $"HTTP NOK: {f}\r\n";
                }

                //log += $"Trying to find file in search folders:\r\n";


                foreach (string d in this.HtmlFolders)
                {
                    f = d + localPath;
                    string lf = d + " => " + localPath;
                    //log += $"\tFOLDER  : {d}\r\n";

                    if (tryReadFile(f, out contents))
                    {

                        res.ContentType = MimeTypes.Mappings[Path.GetExtension(f)];
                        res.ContentLength64 = contents.LongLength;
                        res.Close(contents, true);


                        //log += $"\tHTTP OK: {f}\r\n";
                        //write2log = false;
                        return;

                    }
                    else
                    {
                        //log += $"\tHTTP NOK: {f}\r\n";
                    }
                }

                //foreach (string d in this.MainWindowViewModel.AppSettingsViewModel.WEBSearchFolders)
                //{
                //    f = d + localPath;
                //    string lf = d + " => " + localPath;
                //    log += $"\tFOLDER  : {d}\r\n";

                //    if (tryReadFile(f, out contents))
                //    {

                //        res.ContentType = MimeTypes.Mappings[Path.GetExtension(f)];
                //        res.ContentLength64 = contents.LongLength;
                //        res.Close(contents, true);


                //        log += $"\tHTTP OK: {f}\r\n";
                //        write2log = true;
                //        return;

                //    }
                //    else
                //    {
                //        log += $"\tHTTP NOK: {f}\r\n";
                //    }
                //}


                ////if (e.TryReadFile(path, out contents))
                //{
                //    res.ContentType = MimeTypes.Mappings[Path.GetExtension(path)];
                //    Console.WriteLine("OK       : " + path);
                //    Globals.MsgLogger.Add("Request: " + path);
                //    Globals.MsgLogger.Add("Response: " + Path.Combine(this.DocumentRootPath, path));
                //    Globals.MsgLogger.Add("");
                //    res.ContentLength64 = contents.LongLength;
                //    res.Close(contents, true);
                //    return;
                //}

                

                res.StatusCode = (int)HttpStatusCode.NotFound;
                return;
                //res.ContentLength64 = contents.LongLength;
                //res.Close(contents, true);
            }
            catch (Exception ex)
            { }
            finally
            {
                if(res.StatusCode != (int)HttpStatusCode.OK)
                {

                    log += $"HTTP REQUEST: {path}\r\n";
                    log += "==================================================================================\r\n\r\n";

                    this.MainWindowViewModel.LogViewModel.Add(log);
                }

            }
        }


        private static bool tryReadFile(string path, out byte[] contents)
        {
            contents = null;
          //C:\MSFS2020\Official\OneStore\asobo - vcockpits - instruments - generic\html_ui\Pages\VCockpit\Instruments\Generic\Autopilots\KAP140\KAP140.css
            if (!File.Exists(path))
                return false;

            try
            {
                contents = File.ReadAllBytes(path);
            }
            catch
            {
                return false;
            }

            return true;
        }

        
    }
}
