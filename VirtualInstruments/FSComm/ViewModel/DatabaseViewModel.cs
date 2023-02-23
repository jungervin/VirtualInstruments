using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Newtonsoft.Json;
using System.Windows;
using FSComm.ViewModel;
using FSComm.Model;

namespace FSComm.ViewModel
{
    public class DatabaseViewModel
    {
        private string ConnectioString;


        public DatabaseViewModel(MainWindowViewModel mainwindowviewmodel)
        {
            this.MainWindowViewModel = mainwindowviewmodel;

        }


        public void Init()
        {
            //this.MainWindowViewModel.LogViewModel.Add($"Trying to open the databse: {this.AppSettings.DatabaseFile}");


            if (File.Exists(this.AppSettings.DatabaseFile))
            {
                this.ConnectioString = $"Data Source = {this.AppSettings.DatabaseFile}; Cache = Shared";
            }
            else
            {
              //  this.MainWindowViewModel.LogViewModel.Add("Database does not exists!");
            }

            //this.Airports = this.FillAirports();
            //this.MainWindowViewModel.LogViewModel.Add($"Airports: {this.Airports.Count}");
            //this.Runways = this.FillRunways();
            //this.MainWindowViewModel.LogViewModel.Add($"Runways: {this.Runways.Count}");
            //this.RunwayEnds = this.FillRunwayEnds();
            //this.MainWindowViewModel.LogViewModel.Add($"RunwayEnds: {this.RunwayEnds.Count}");
            //this.Parkings = this.FillParkings();
            //this.MainWindowViewModel.LogViewModel.Add($"Parkings: {this.Parkings.Count}");
            //this.ILSs = this.FillILSs();
            //this.MainWindowViewModel.LogViewModel.Add($"ILS: {this.ILSs.Count}");
            //this.Approaches = this.FillApproaches();
            //this.MainWindowViewModel.LogViewModel.Add($"Approaches: {this.Approaches.Count}");
            //this.ApproachLegs = this.FillApproachLegs();
            //this.MainWindowViewModel.LogViewModel.Add($"ApproachLegs: {this.ApproachLegs.Count}");
            //this.Waypoints = this.FillWaypoints();
            //this.MainWindowViewModel.LogViewModel.Add($"Waypoints: {this.Waypoints.Count}");
            //this.VORs = this.FillVORs();
            //this.MainWindowViewModel.LogViewModel.Add($"VORs: {this.VORs.Count}");
            //this.NDBs = this.FillNDBs();
            //this.MainWindowViewModel.LogViewModel.Add($"NDBs: {this.NDBs.Count}");
            //this.NavSearch = this.FillNavSearch();
            //this.MainWindowViewModel.LogViewModel.Add($"NavSearchs: {this.NavSearch.Count}");
            //this.Transitions = this.FillTransitions();
            //this.MainWindowViewModel.LogViewModel.Add($"Transitions: {this.Transitions.Count}");
            //this.TransitionLegs = this.FillTransitionLegs();
            //this.MainWindowViewModel.LogViewModel.Add($"TransitionLegs: {this.TransitionLegs.Count}");
        }

        public List<AirPort> SelectAirPorts(MapDetailsModel d)
        {
            return this.Airports.Where(k =>
                k.rating >= d.rating &&
                k.laty >= d.bounds._southWest.lat &&
                k.laty <= d.bounds._northEast.lat &&
                k.lonx >= d.bounds._southWest.lng &&
                k.lonx <= d.bounds._northEast.lng

            ).ToList();
        }


        public List<VOR> SelectVORs(MapDetailsModel d)
        {
            return this.VORs.Where(k =>
                k.laty >= d.bounds._southWest.lat &&
                k.laty <= d.bounds._northEast.lat &&
                k.lonx >= d.bounds._southWest.lng &&
                k.lonx <= d.bounds._northEast.lng

            ).ToList();
        }
        public List<NDB> SelectNDBs(MapDetailsModel d)
        {
            return this.NDBs.Where(k =>
                k.laty >= d.bounds._southWest.lat &&
                k.laty <= d.bounds._northEast.lat &&
                k.lonx >= d.bounds._southWest.lng &&
                k.lonx <= d.bounds._northEast.lng

            ).ToList();
        }


        public AirPort SelectAirPort(string ident)
        {
            return this.Airports.Where(k => k.ident == ident).FirstOrDefault();
        }


        public List<Parking> SelectParkings(MapDetailsModel d)
        {
            return this.Parkings.Where(k =>
                 k.laty >= d.bounds._southWest.lat &&
                 k.laty <= d.bounds._northEast.lat &&
                 k.lonx >= d.bounds._southWest.lng &&
                 k.lonx <= d.bounds._northEast.lng
             ).ToList();
        }

        public VOR SelectVOR(Int64 id)
        {
            return this.VORs.Where(k => k.vor_id == id).FirstOrDefault();
        }

        public AirPortsInfo GetAiportInfo(string ident)
        {
            AirPortsInfo ai = new AirPortsInfo();
            //ai.AirPort = this.Airports.Where(k => k.ident == ident).FirstOrDefault();
            //ai.Runways = new List<RunwayInfo>();

            //List<Runway> runways = this.Runways.Where(k => k.airport_id == ai.AirPort.airport_id).ToList();
            //foreach (Runway rw in runways)
            //{
            //    RunwayInfo ri = new RunwayInfo();
            //    ri.Runway = rw;

            //    ri.PrimaryEnd = this.RunwayEnds.Where(k => k.runway_end_id == rw.primary_end_id).FirstOrDefault();
            //    ri.SecondaryEnd = this.RunwayEnds.Where(k => k.runway_end_id == rw.secondary_end_id).FirstOrDefault();
            //    ai.Runways.Add(ri);
            //}
            return ai;
        }

        public string ConnectionString { get; set; }

        public List<AirPort> Airports { get; set; }
        public List<Runway> Runways { get; set; }
        public List<RunwayEnd> RunwayEnds { get; set; }
        public List<Parking> Parkings { get; set; }
        public List<ILS> ILSs { get; set; }
        public List<Approach> Approaches { get; set; }
        public List<ApproachLeg> ApproachLegs { get; set; }
        public List<Waypoint> Waypoints { get; set; }
        public List<VOR> VORs { get; set; }
        public List<NDB> NDBs { get; set; }
        public List<NavSearch> NavSearch { get; set; }
        public List<Transition> Transitions { get; set; }
        public List<TransitionLeg> TransitionLegs { get; set; }


        public List<AirPort> FillAirports(string filter)
        {
            List<AirPort> list = new List<AirPort>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM AIRPORT WHERE ident LIKE '{filter}%'", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            list.Add(AirPort.FromDataReader(r));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.FillAirpors()", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return list;
        }

        public List<Runway> FillRunways(long airport_id)
        {

            List<Runway> list = new List<Runway>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM RUNWAY WHERE airport_id = { airport_id }", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            list.Add(Runway.FromDataReader(r));

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.FillRunways()", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return list;
        }


        public List<RunwayEnd> FillRunwayEnds(long primary_end_id)
        {
            List<RunwayEnd> list = new List<RunwayEnd>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM RUNWAY_END WHERE runway_end_id = {primary_end_id}", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            list.Add(RunwayEnd.FromDataReader(r));

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.FillRunwaysEnd()", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return list;
        }

        public List<Parking> FillParkings()
        {
            List<Parking> list = new List<Parking>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM Parking", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            Parking pa = new Parking();

                            pa.parking_id = r.GetValue(0) != DBNull.Value ? r.GetInt64(0) : 0;
                            pa.airport_id = r.GetValue(1) != DBNull.Value ? r.GetInt64(1) : 0;
                            pa.type = r.GetValue(2) != DBNull.Value ? r.GetString(2) : "";
                            pa.pushback = r.GetValue(3) != DBNull.Value ? r.GetString(3) : "";
                            pa.name = r.GetValue(4) != DBNull.Value ? r.GetString(4) : "";
                            pa.number = r.GetValue(5) != DBNull.Value ? r.GetInt32(5) : 0;
                            pa.airline_codes = r.GetValue(6) != DBNull.Value ? r.GetString(6) : "";
                            pa.radius = r.GetValue(7) != DBNull.Value ? r.GetDouble(7) : 0;
                            pa.heading = r.GetValue(8) != DBNull.Value ? r.GetDouble(8) : 0;
                            pa.has_jetway = r.GetValue(9) != DBNull.Value ? r.GetInt64(9) : 0;
                            pa.lonx = r.GetValue(10) != DBNull.Value ? r.GetDouble(10) : 0;
                            pa.laty = r.GetValue(11) != DBNull.Value ? r.GetDouble(11) : 0;

                            list.Add(pa);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.FillParkings()", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            return list;
        }

        public List<ILS> FillILSs(string ils_ident)
        {
            List<ILS> list = new List<ILS>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM ILS WHERE ident = '{ils_ident}'", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            list.Add(ILS.FromDataReader(r));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.FillILSs()", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return list;

        }

        public List<NDB> FillNDBs()
        {
            List<NDB> list = new List<NDB>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM NDB", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            NDB ndb = new NDB();
                            var i = 0;
                            ndb.ndb_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ndb.file_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ndb.ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ndb.name = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ndb.region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ndb.airport_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ndb.airport_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ndb.type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ndb.frequency = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ndb.range = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ndb.mag_var = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            ndb.altitude = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ndb.lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            ndb.laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;



                            list.Add(ndb);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.FillNDBs()", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return list;
        }

        public List<VOR> FillVORs()
        {
            List<VOR> list = new List<VOR>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM VOR", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            VOR vo = new VOR();
                            var i = 0;
                            vo.vor_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            vo.file_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            vo.ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            vo.name = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            vo.region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            vo.airport_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            vo.airport_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            vo.type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            vo.frequency = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            vo.channel = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            vo.range = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            vo.mag_var = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            vo.dme_only = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            vo.dme_altitude = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            vo.dme_lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            vo.dme_laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            vo.altitude = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            vo.lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            vo.laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;

                            list.Add(vo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.FillVORs()", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return list;
        }

        public List<Approach> FillApproaches(long airport_id)
        {
            List<Approach> list = new List<Approach>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM APPROACH where airport_id = {airport_id}", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            Approach apr = new Approach();
                            int i = 0;
                            apr.approach_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            apr.airport_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            apr.runway_end_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            apr.arinc_name = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            apr.airport_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            apr.runway_name = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            apr.type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            apr.suffix = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            apr.has_gps_overlay = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            apr.fix_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            apr.fix_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            apr.fix_region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            apr.fix_airport_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            apr.aircraft_category = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            apr.altitude = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            apr.heading = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            apr.missed_altitude = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;

                            list.Add(apr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.FillApproaches()", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return list;
        }

        public List<ApproachLeg> FillApproachLegs(long approach_id)
        {
            List<ApproachLeg> list = new List<ApproachLeg>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM APPROACH_LEG WHERE approach_id = { approach_id }", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            ApproachLeg leg = new ApproachLeg();
                            int i = 0;

                            leg.approach_leg_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            leg.approach_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            leg.is_missed = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            leg.type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.arinc_descr_code = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.approach_fix_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.alt_descriptor = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.turn_direction = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.rnp = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;

                            leg.fix_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.fix_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.fix_region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.fix_airport_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.fix_lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            leg.fix_laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            leg.recommended_fix_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.recommended_fix_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.recommended_fix_region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.recommended_fix_lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            leg.recommended_fix_laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            leg.is_flyover = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            leg.is_true_course = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            leg.course = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            leg.distance = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            leg.time = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            leg.theta = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            leg.rho = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            leg.altitude1 = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            leg.altitude2 = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            leg.speed_limit_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            leg.speed_limit = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            leg.vertical_angle = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;

                            list.Add(leg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.FillApproachLegs()", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return list;
        }

        public List<Waypoint> FillWaypoints(string fix_ident, string fix_region)
        {
            List<Waypoint> list = new List<Waypoint>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM WAYPOINT WHERE ident = '{fix_ident}' AND region = '{fix_region}'", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            Waypoint wp = new Waypoint();
                            int i = 0;

                            wp.waypoint_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            wp.file_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            wp.nav_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            wp.ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            wp.region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            wp.airport_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            wp.airport_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            wp.artificial = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            wp.type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            wp.arinc_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            wp.num_victor_airway = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            wp.num_jet_airway = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            wp.mag_var = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            wp.lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            wp.laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;

                            list.Add(wp);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.FillWaypoints()", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return list;
        }

        public List<NavSearch> FillNavSearch(string fix_ident, string fix_region)
        {
            List<NavSearch> list = new List<NavSearch>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM NAV_SEARCH WHERE ident = '{fix_ident}' AND region = '{fix_region}'", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            NavSearch ns = new NavSearch();
                            int i = 0;

                            ns.nav_search_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ns.waypoint_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ns.waypoint_nav_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ns.vor_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ns.ndb_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ns.file_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ns.airport_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ns.airport_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ns.ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ns.name = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ns.region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ns.range = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ns.type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ns.nav_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ns.arinc_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ns.frequency = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ns.channel = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ns.waypoint_num_victor_airway = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ns.waypoint_num_jet_airway = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ns.scenery_local_path = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            //ns.bgl_filename = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            ns.mag_var = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            ns.altitude = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            ns.lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            ns.laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;

                            list.Add(ns);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.NavSearch()", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return list;
        }

        public List<Transition> FillTransitions(long approach_id)
        {
            List<Transition> list = new List<Transition>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM TRANSITION WHERE approach_id = {approach_id}", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            Transition tr = new Transition();
                            int i = 0;

                            tr.transition_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            tr.approach_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            tr.type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.fix_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.fix_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.fix_region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.fix_airport_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.aircraft_category = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.altitude = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            tr.dme_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.dme_region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            //object? dme_airport_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.dme_radial = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.dme_distance = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;



                            list.Add(tr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.NavSearch()", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return list;
        }

        public List<TransitionLeg> FillTransitionLegs(long transition_id)
        {
            List<TransitionLeg> list = new List<TransitionLeg>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM TRANSITION_LEG WHERE transition_id = {transition_id}", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            TransitionLeg tr = new TransitionLeg();
                            int i = 0;

                            tr.transition_leg_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            tr.transition_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            tr.type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.arinc_descr_code = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.approach_fix_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.alt_descriptor = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.turn_direction = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.rnp = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.fix_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.fix_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.fix_region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.fix_airport_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.fix_lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.fix_laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.recommended_fix_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.recommended_fix_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.recommended_fix_region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.recommended_fix_lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.recommended_fix_laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.is_flyover = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            tr.is_true_course = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            tr.course = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.distance = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.time = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.theta = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.rho = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.altitude1 = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.altitude2 = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
                            i++;
                            tr.speed_limit_type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
                            i++;
                            tr.speed_limit = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
                            i++;
                            tr.vertical_angle = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;


                            list.Add(tr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DatabaseViewModel.NavSearch()", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return list;
        }

        public MainWindowViewModel MainWindowViewModel { get; }

        private AppSettingsViewModel AppSettings
        {
            get { return this.MainWindowViewModel.AppSettingsViewModel; }
        }

    }
}
