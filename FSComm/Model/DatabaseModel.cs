using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSComm.Model
{
    public class DataModel
    {
        public string cmd { get; set; }
        public string data { get; set; }
    }


    public class QueryModel
    {
        public string cmd { get; set; }
        public object data { get; set; }
    }

    public class AirPort
    {
        public AirPort()
        {
        }

        public string otype { get { return "airport"; } }
        public long airport_id { get; set; }
        public long file_id { get; set; }
        public string ident { get; set; }
        public string icao { get; set; }
        public string iata { get; set; }

        public string faa { get; set; }
        public string local { get; set; }
        //public string xpident { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public long flatten { get; set; }

        public long type { get; set; }

        public long fuel_flags { get; set; }
        public long has_avgas { get; set; }
        public long has_jetfuel { get; set; }
        public long has_tower_object { get; set; }
        public long tower_frequency { get; set; }
        public long atis_frequency { get; set; }
        public long awos_frequency { get; set; }
        public long asos_frequency { get; set; }
        public long unicom_frequency { get; set; }
        public long is_closed { get; set; }
        public long is_military { get; set; }
        public long is_addon { get; set; }
        public long num_com { get; set; }
        public long num_parking_gate { get; set; }
        public long num_parking_ga_ramp { get; set; }
        public long num_parking_cargo { get; set; }
        public long num_parking_mil_cargo { get; set; }
        public long num_parking_mil_combat { get; set; }
        public long num_approach { get; set; }
        public long num_runway_hard { get; set; }
        public long num_runway_soft { get; set; }
        public long num_runway_water { get; set; }
        public long num_runway_light { get; set; }
        public long num_runway_end_closed { get; set; }
        public long num_runway_end_vasi { get; set; }
        public long num_runway_end_als { get; set; }
        public long num_runway_end_ils { get; set; }
        public long num_apron { get; set; }
        public long num_taxi_path { get; set; }
        public long num_helipad { get; set; }
        public long num_jetway { get; set; }
        public long num_starts { get; set; }
        public long longest_runway_length { get; set; }
        public long longest_runway_width { get; set; }
        public double longest_runway_heading { get; set; }
        public string longest_runway_surface { get; set; }
        public long num_runways { get; set; }
        public string largest_parking_ramp { get; set; }
        public string largest_parking_gate { get; set; }
        public long rating { get; set; }
        public long is_3d { get; set; }
        public string scenery_local_path { get; set; }
        //public string bgl_filename { get; set; }
        public double left_lonx { get; set; }
        public double top_laty { get; set; }
        public double right_lonx { get; set; }
        public double bottom_laty { get; set; }
        public double mag_var { get; set; }
        public long tower_altitude { get; set; }
        public double tower_lonx { get; set; }
        public double tower_laty { get; set; }
        public long transition_altitude { get; set; }
        public long altitude { get; set; }
        public double lonx { get; set; }
        public double laty { get; set; }

        public List<Runway> Runways
        {
            //get
            //{
            //    //if (Globals.MainWindow.DatabaseViewModel.Runways != null)
            //    //{
            //    //    return Globals.MainWindow.DatabaseViewModel.Runways.Where(k => k.airport_id == this.airport_id).ToList();
            //    //}
            //    return null;
            //}
            get; set;
        }

        public List<Approach> Approaches
        {
            get; set;
        }

        public static AirPort FromDataReader(SQLiteDataReader r)
        {
            AirPort a = new AirPort();
            int i = 0;
            a.airport_id = r.GetInt64(i);
            i++;
            a.file_id = r.GetInt64(i);
            i++;
            a.ident = r.GetString(i);
            i++;
            a.icao = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            a.iata = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            //a.xpident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            //i++;
            //i = 7;
            a.faa = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            a.local = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;



            a.name = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            a.city = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            a.state = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            a.country = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            a.region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            a.flatten = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;

            a.type = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;

            a.fuel_flags = r.GetInt64(i);
            i++;
            a.has_avgas = r.GetInt64(i);
            i++;
            a.has_jetfuel = r.GetInt64(i);
            i++;
            a.has_tower_object = r.GetInt64(i);
            i++;
            a.tower_frequency = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            a.atis_frequency = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            a.awos_frequency = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            a.asos_frequency = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            a.unicom_frequency = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            a.is_closed = r.GetInt64(i);
            i++;
            a.is_military = r.GetInt64(i);
            i++;
            a.is_addon = r.GetInt64(i);
            i++;
            a.num_com = r.GetInt64(i);
            i++;
            a.num_parking_gate = r.GetInt64(i);
            i++;
            a.num_parking_ga_ramp = r.GetInt64(i);
            i++;
            a.num_parking_cargo = r.GetInt64(i);
            i++;
            a.num_parking_mil_cargo = r.GetInt64(i);
            i++;
            a.num_parking_mil_combat = r.GetInt64(i);
            i++;
            a.num_approach = r.GetInt64(i);
            i++;
            a.num_runway_hard = r.GetInt64(i);
            i++;
            a.num_runway_soft = r.GetInt64(i);
            i++;
            a.num_runway_water = r.GetInt64(i);
            i++;
            a.num_runway_light = r.GetInt64(i);
            i++;
            a.num_runway_end_closed = r.GetInt64(i);
            i++;
            a.num_runway_end_vasi = r.GetInt64(i);
            i++;
            a.num_runway_end_als = r.GetInt64(i);
            i++;
            a.num_runway_end_ils = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            a.num_apron = r.GetInt64(i);
            i++;
            a.num_taxi_path = r.GetInt64(i);
            i++;
            a.num_helipad = r.GetInt64(i);
            i++;
            a.num_jetway = r.GetInt64(i);
            i++;
            a.num_starts = r.GetInt64(i);
            i++;
            a.longest_runway_length = r.GetInt64(i);
            i++;
            a.longest_runway_width = r.GetInt64(i);
            i++;
            a.longest_runway_heading = r.GetDouble(i);
            i++;
            a.longest_runway_surface = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            a.num_runways = r.GetInt64(i);
            i++;
            a.largest_parking_ramp = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            a.largest_parking_gate = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            a.rating = r.GetInt64(i);
            i++;
            a.is_3d = r.GetInt64(i);
            i++;
            a.scenery_local_path = r.GetString(i);
            i++;
            //a.bgl_filename = r.GetString(i);
            i++;
            a.left_lonx = r.GetDouble(i);
            i++;
            a.top_laty = r.GetDouble(i);
            i++;
            a.right_lonx = r.GetDouble(i);
            i++;
            a.bottom_laty = r.GetDouble(i);
            i++;
            a.mag_var = r.GetDouble(i);
            i++;
            a.tower_altitude = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            a.tower_lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            a.tower_laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            a.transition_altitude = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            a.altitude = r.GetInt64(i);
            i++;
            a.lonx = r.GetDouble(i);
            i++;
            a.laty = r.GetDouble(i);


            return a;
        }
    }

    public class Runway
    {
        public Runway()
        {
        }

        public long runway_id { get; set; }
        public long airport_id { get; set; }
        public long primary_end_id { get; set; }
        public long secondary_end_id { get; set; }
        public string surface { get; set; }
        public double smoothness { get; set; }
        public string shoulder { get; set; }
        public double length { get; set; }
        public double width { get; set; }
        public double heading { get; set; }
        public long pattern_altitude { get; set; }
        public long marking_flags { get; set; }
        public string edge_light { get; set; }
        public string center_light { get; set; }
        public long has_center_red { get; set; }
        public double primary_lonx { get; set; }
        public double primary_laty { get; set; }
        public double secondary_lonx { get; set; }
        public double secondary_laty { get; set; }
        public long altitude { get; set; }
        public double lonx { get; set; }
        public double laty { get; set; }


        public RunwayEnd Primary
        {
            //get
            //{
            //    //if (Globals.MainWindow.DatabaseViewModel.RunwayEnds != null)
            //    //{
            //    //    return Globals.MainWindow.DatabaseViewModel.RunwayEnds.Where(k => k.runway_end_id == this.primary_end_id).FirstOrDefault();
            //    //}
            //    return null;
            //}
            get; set;
        }
        public RunwayEnd Secondary
        {
            //get
            //{
            //    //if (Globals.MainWindow.DatabaseViewModel.RunwayEnds != null)
            //    //{
            //    //    return Globals.MainWindow.DatabaseViewModel.RunwayEnds.Where(k => k.runway_end_id == this.secondary_end_id).FirstOrDefault();
            //    //}
            //    return null;
            //}
            get; set;

        }

        public static Runway FromDataReader(SQLiteDataReader r)
        {
            Runway rw = new Runway();

            rw.runway_id = r.GetValue(0) != DBNull.Value ? r.GetInt64(0) : 0;
            rw.airport_id = r.GetValue(1) != DBNull.Value ? r.GetInt64(1) : 0;
            rw.primary_end_id = r.GetValue(2) != DBNull.Value ? r.GetInt64(2) : 0;
            rw.secondary_end_id = r.GetValue(3) != DBNull.Value ? r.GetInt64(3) : 0;
            rw.surface = r.GetValue(4) != DBNull.Value ? r.GetString(4) : "";
            rw.smoothness = r.GetValue(5) != DBNull.Value ? r.GetDouble(5) : 0;
            rw.shoulder = r.GetValue(6) != DBNull.Value ? r.GetString(6) : "";
            rw.length = r.GetValue(7) != DBNull.Value ? r.GetDouble(7) : 0;
            rw.width = r.GetValue(8) != DBNull.Value ? r.GetDouble(8) : 0;
            rw.heading = r.GetValue(9) != DBNull.Value ? r.GetDouble(9) : 0;
            rw.pattern_altitude = r.GetValue(10) != DBNull.Value ? r.GetInt64(10) : 0;
            rw.marking_flags = r.GetValue(11) != DBNull.Value ? r.GetInt64(11) : 0;
            rw.edge_light = r.GetValue(12) != DBNull.Value ? r.GetString(12) : "";
            rw.center_light = r.GetValue(13) != DBNull.Value ? r.GetString(13) : "";
            rw.has_center_red = r.GetValue(14) != DBNull.Value ? r.GetInt64(14) : 0;
            rw.primary_lonx = r.GetValue(15) != DBNull.Value ? r.GetDouble(15) : 0;
            rw.primary_laty = r.GetValue(16) != DBNull.Value ? r.GetDouble(16) : 0;
            rw.secondary_lonx = r.GetValue(17) != DBNull.Value ? r.GetDouble(17) : 0;
            rw.secondary_laty = r.GetValue(18) != DBNull.Value ? r.GetDouble(18) : 0;
            rw.altitude = r.GetValue(19) != DBNull.Value ? r.GetInt64(19) : 0;
            rw.lonx = r.GetValue(20) != DBNull.Value ? r.GetDouble(20) : 0;
            rw.laty = r.GetValue(21) != DBNull.Value ? r.GetDouble(21) : 0;

            return rw;
        }
    }

    public class RunwayEnd
    {
        public RunwayEnd()
        {
        }

        public long runway_end_id { get; set; }
        public string name { get; set; }
        public string end_type { get; set; }
        public double offset_threshold { get; set; }
        public double blast_pad { get; set; }
        public double overrun { get; set; }
        public string left_vasi_type { get; set; }
        public double left_vasi_pitch { get; set; }
        public string right_vasi_type { get; set; }
        public double right_vasi_pitch { get; set; }
        public long has_closed_markings { get; set; }
        public long has_stol_markings { get; set; }
        public long is_takeoff { get; set; }
        public long is_landing { get; set; }
        public string is_pattern { get; set; }
        public string app_light_system_type { get; set; }
        public long has_end_lights { get; set; }
        public long has_reils { get; set; }
        public long has_touchdown_lights { get; set; }
        public long num_strobes { get; set; }
        public string ils_ident { get; set; }
        public double heading { get; set; }
        public long altitude { get; set; }
        public double lonx { get; set; }
        public double laty { get; set; }

        //public List<ILS> ILS
        public ILS ILS
        {
            //get
            //{
            //    //if (Globals.MainWindow.DatabaseViewModel.ILSs != null && this.ils_ident != null)
            //    //{
            //    //    return Globals.MainWindow.DatabaseViewModel.ILSs.Where(k => k.ident == this.ils_ident && k.loc_runway_end_id == this.runway_end_id).FirstOrDefault();
            //    //}
            //    return null;
            //}
            get; set;
        }

        public static RunwayEnd FromDataReader(SQLiteDataReader r)
        {
            RunwayEnd re = new RunwayEnd();

            re.runway_end_id = r.GetValue(0) != DBNull.Value ? r.GetInt64(0) : 0;
            re.name = r.GetValue(1) != DBNull.Value ? r.GetString(1) : "";
            re.end_type = r.GetValue(2) != DBNull.Value ? r.GetString(2) : "";
            re.offset_threshold = r.GetValue(3) != DBNull.Value ? r.GetDouble(3) : 0;
            re.blast_pad = r.GetValue(4) != DBNull.Value ? r.GetDouble(4) : 0;
            re.overrun = r.GetValue(5) != DBNull.Value ? r.GetDouble(5) : 0;
            re.left_vasi_type = r.GetValue(6) != DBNull.Value ? r.GetString(6) : "";
            re.left_vasi_pitch = r.GetValue(7) != DBNull.Value ? r.GetDouble(7) : 0;
            re.right_vasi_type = r.GetValue(8) != DBNull.Value ? r.GetString(8) : "";
            re.right_vasi_pitch = r.GetValue(9) != DBNull.Value ? r.GetDouble(9) : 0;
            re.has_closed_markings = r.GetValue(10) != DBNull.Value ? r.GetInt64(10) : 0;
            re.has_stol_markings = r.GetValue(11) != DBNull.Value ? r.GetInt64(11) : 0;
            re.is_takeoff = r.GetValue(12) != DBNull.Value ? r.GetInt64(12) : 0;
            re.is_landing = r.GetValue(13) != DBNull.Value ? r.GetInt64(13) : 0;
            re.is_pattern = r.GetValue(14) != DBNull.Value ? r.GetString(14) : "";
            re.app_light_system_type = r.GetValue(15) != DBNull.Value ? r.GetString(15) : "";
            re.has_end_lights = r.GetValue(16) != DBNull.Value ? r.GetInt64(16) : 0;
            re.has_reils = r.GetValue(17) != DBNull.Value ? r.GetInt64(17) : 0;
            re.has_touchdown_lights = r.GetValue(18) != DBNull.Value ? r.GetInt64(18) : 0;
            re.num_strobes = r.GetValue(19) != DBNull.Value ? r.GetInt64(19) : 0;
            re.ils_ident = r.GetValue(20) != DBNull.Value ? r.GetString(20) : "";
            re.heading = r.GetValue(21) != DBNull.Value ? r.GetDouble(21) : 0;
            re.altitude = r.GetValue(22) != DBNull.Value ? r.GetInt64(22) : 0;
            re.lonx = r.GetValue(23) != DBNull.Value ? r.GetDouble(23) : 0;
            re.laty = r.GetValue(24) != DBNull.Value ? r.GetDouble(24) : 0;

            return re;
        }
    }

    public class Parking
    {
        public Parking()
        { }

        public long parking_id { get; set; }
        public long airport_id { get; set; }
        public string type { get; set; }
        public string pushback { get; set; }
        public string name { get; set; }
        public long number { get; set; }
        public string airline_codes { get; set; }
        public double radius { get; set; }
        public double heading { get; set; }
        public long has_jetway { get; set; }
        public double lonx { get; set; }
        public double laty { get; set; }
    }

    public class ILS
    {
        public long ils_id { get; set; }
        public string ident { get; set; }
        public string name { get; set; }
        public string region { get; set; }
        public string type { get; set; }

        public string perf_indicator { get; internal set; }
        public string provider { get; internal set; }

        public long frequency { get; set; }
        public long range { get; set; }
        public double mag_var { get; set; }
        public long has_backcourse { get; set; }
        public long dme_range { get; set; }
        public long dme_altitude { get; set; }
        public double dme_lonx { get; set; }
        public double dme_laty { get; set; }
        public long gs_range { get; set; }
        public double gs_pitch { get; set; }
        public long gs_altitude { get; set; }
        public double gs_lonx { get; set; }
        public double gs_laty { get; set; }
        public long loc_runway_end_id { get; set; }
        public string loc_airport_ident { get; set; }
        public string loc_runway_name { get; set; }
        public double loc_heading { get; set; }
        public double loc_width { get; set; }
        public double end1_lonx { get; set; }
        public double end1_laty { get; set; }
        public double end_mid_lonx { get; set; }
        public double end_mid_laty { get; set; }
        public double end2_lonx { get; set; }
        public double end2_laty { get; set; }
        public long altitude { get; set; }
        public double lonx { get; set; }
        public double laty { get; set; }

        public static ILS FromDataReader(SQLiteDataReader r)
        {
            var i = 0;
            ILS ils = new ILS();

            ils.ils_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            ils.ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            ils.name = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            ils.region = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            ils.type = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            ils.perf_indicator = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            ils.provider = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            ils.frequency = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            ils.range = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            ils.mag_var = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.has_backcourse = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            ils.dme_range = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            ils.dme_altitude = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            ils.dme_lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.dme_laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.gs_range = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            ils.gs_pitch = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.gs_altitude = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            ils.gs_lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.gs_laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.loc_runway_end_id = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            ils.loc_airport_ident = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            ils.loc_runway_name = r.GetValue(i) != DBNull.Value ? r.GetString(i) : "";
            i++;
            ils.loc_heading = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.loc_width = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.end1_lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.end1_laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.end_mid_lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.end_mid_laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.end2_lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.end2_laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.altitude = r.GetValue(i) != DBNull.Value ? r.GetInt64(i) : 0;
            i++;
            ils.lonx = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;
            ils.laty = r.GetValue(i) != DBNull.Value ? r.GetDouble(i) : 0;
            i++;

            return ils;
        }
    }

    public class VOR
    {
        public VOR()
        { }

        public long vor_id { get; set; }
        public long file_id { get; set; }
        public string ident { get; set; }
        public string name { get; set; }
        public string region { get; set; }
        public long airport_id { get; set; }
        public string type { get; set; }
        public long frequency { get; set; }
        public string channel { get; set; }
        public long range { get; set; }
        public double mag_var { get; set; }
        public long dme_only { get; set; }
        public long dme_altitude { get; set; }
        public double dme_lonx { get; set; }
        public double dme_laty { get; set; }
        public long altitude { get; set; }
        public double lonx { get; set; }
        public double laty { get; set; }
        public object airport_ident { get; set; }
    }

    public class NDB
    {
        public long ndb_id { get; set; }
        public long file_id { get; set; }
        public string ident { get; set; }
        public string name { get; set; }
        public string region { get; set; }
        public long airport_id { get; set; }
        public string airport_ident { get; set; }
        public string type { get; set; }
        public long frequency { get; set; }
        public long range { get; set; }
        public double mag_var { get; set; }
        public long altitude { get; set; }
        public double lonx { get; set; }
        public double laty { get; set; }

    }

    public class AirPortInfo
    {
        public AirPortInfo()
        {

        }

        public string type { get { return "airport"; } }
        public AirPort AirPort { get; set; }

    }

    public class AirPortsInfo
    {
        public AirPortsInfo()
        {

        }

        public string type { get { return "airports"; } }
        public List<AirPort> AirPorts { get; set; }

    }

    public class VORInfo
    {
        public string data_type { get { return "vors"; } }
        public List<VOR> VORs { get; set; }

    }
    public class NDBInfo
    {
        public string data_type { get { return "ndbs"; } }
        public List<NDB> NDBs { get; set; }

    }



    public class RunwayInfo
    {
        public Runway Runway { get; set; }
        public RunwayEnd PrimaryEnd { get; set; }
        public RunwayEnd SecondaryEnd { get; set; }

    }


    public class ResponseRunways
    {
        public string type { get; set; }
        public List<RunwayInfo> runways { get; set; }
    }

    public class ResponseParkings
    {
        public string type { get; set; }
        public List<Parking> parkings { get; set; }
    }

    public class ResponseVORs
    {
        public string type { get { return "vors"; } }
        public List<VOR> vors { get; set; }
    }




    public class MapDetailsModel
    {
        public int zoom { get; set; }
        public int rating { get; set; }
        public MapBoundModel bounds { get; set; }
    }


    public class MapCoordModel
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class MapBoundModel
    {
        public MapCoordModel _southWest { get; set; }
        public MapCoordModel _northEast { get; set; }
    }

    public class Approach
    {
        public long approach_id { get; set; }
        public long airport_id { get; set; }
        public long runway_end_id { get; set; }
        public string arinc_name { get; set; }
        public string airport_ident { get; set; }
        public string runway_name { get; set; }
        public string type { get; set; }
        public string suffix { get; set; }
        public long has_gps_overlay { get; set; }
        public string fix_type { get; set; }
        public string fix_ident { get; set; }
        public string fix_region { get; set; }
        public string fix_airport_ident { get; set; }
        public string aircraft_category { get; set; }

        public long altitude { get; set; }
        public double heading { get; set; }
        public long missed_altitude { get; set; }

        public List<ApproachLeg> Legs { get; set; }
        public List<Transition> Transitions { get; set; }
    }

    public class ApproachLeg
    {
        public long approach_leg_id { get; set; }
        public long approach_id { get; set; }
        public long is_missed { get; set; }
        public string type { get; set; }
        public string arinc_descr_code { get; set; }
        public string approach_fix_type { get; set; }
        public string alt_descriptor { get; set; }
        public string turn_direction { get; set; }
        public double rnp { get; set; }
        public string fix_type { get; set; }
        public string fix_ident { get; set; }
        public string fix_region { get; set; }
        public string fix_airport_ident { get; set; }
        public double fix_lonx { get; set; }
        public double fix_laty { get; set; }
        public string recommended_fix_type { get; set; }
        public string recommended_fix_ident { get; set; }
        public string recommended_fix_region { get; set; }
        public double recommended_fix_lonx { get; set; }
        public double recommended_fix_laty { get; set; }
        public long is_flyover { get; set; }
        public long is_true_course { get; set; }
        public double course { get; set; }
        public double distance { get; set; }
        public double time { get; set; }
        public double theta { get; set; }
        public double rho { get; set; }
        public double altitude1 { get; set; }
        public double altitude2 { get; set; }
        public string speed_limit_type { get; set; }
        public long speed_limit { get; set; }
        public double vertical_angle { get; set; }

        public NavSearch nav_search { get; set; }
    }

    public class Waypoint
    {
        public long waypoint_id { get; set; }
        public long file_id { get; set; }
        public long nav_id { get; set; }
        public string ident { get; set; }
        public string region { get; set; }
        public long airport_id { get; set; }
        public string airport_ident { get; set; }
        public long artificial { get; set; }
        public string type { get; set; }
        public string arinc_type { get; set; }
        public long num_victor_airway { get; set; }
        public long num_jet_airway { get; set; }
        public double mag_var { get; set; }
        public double lonx { get; set; }
        public double laty { get; set; }
    }

    public class NavSearch
    {
        public long waypoint_id { get; set; }
        public long waypoint_nav_id { get; set; }
        public long vor_id { get; set; }
        public long ndb_id { get; set; }
        public long file_id { get; set; }
        public long airport_id { get; set; }
        public string airport_ident { get; set; }
        public string ident { get; set; }
        public string name { get; set; }
        public string region { get; set; }
        public long range { get; set; }
        public long nav_search_id { get; set; }
        public string type { get; set; }
        public string nav_type { get; set; }
        public string arinc_type { get; set; }
        public long frequency { get; set; }
        public string channel { get; set; }
        public long waypoint_num_victor_airway { get; set; }
        public long waypoint_num_jet_airway { get; set; }
        public string scenery_local_path { get; set; }
       // public string bgl_filename { get; set; }
        public double mag_var { get; set; }
        public long altitude { get; set; }
        public double lonx { get; set; }
        public double laty { get; set; }
    }

    public class Transition
    {
        public long transition_id { get; set; }
        public long approach_id { get; set; }
        public string type { get; set; }
        public string fix_type { get; set; }
        public string fix_ident { get; set; }
        public string fix_region { get; set; }
        public string fix_airport_ident { get; set; }
        public string aircraft_category { get; set; }
        public long altitude { get; set; }
        public string dme_ident { get; set; }
        public string dme_region { get; set; }
        //public string? dme_airport_ident { get; set; }
        public double dme_radial { get; set; }
        public long dme_distance { get; set; }
        public double lonx { get; set; } = 0;
        public double laty { get; set; } = 0;

        public List<TransitionLeg> Legs { get; set; }

    }


    public class TransitionLeg
    {
        public long transition_leg_id;
        public long transition_id;
        public string type;
        public string arinc_descr_code;
        public string approach_fix_type;
        public string alt_descriptor;
        public string turn_direction;
        public double rnp;
        public string fix_type;
        public string fix_ident;
        public string fix_region;
        public string fix_airport_ident;
        public double fix_lonx;
        public double fix_laty;
        public string recommended_fix_type;
        public string recommended_fix_ident;
        public string recommended_fix_region;
        public double recommended_fix_lonx;
        public double recommended_fix_laty;
        public long is_flyover;
        public long is_true_course;
        public double course;
        public double distance;
        public double time;
        public double theta;
        public double rho;
        public double altitude1;
        public double altitude2;
        public string speed_limit_type;
        public long speed_limit;
        public double vertical_angle;
    }
}