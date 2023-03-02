namespace webapioperaciones.Models
{
    public class DatosResumenDiaModel
    {
        public double MaxSpeed { get; set; }
        public double SumDistTot { get; set; }
        public double TimeProd { get; set; }
        public double TimeRecoProd { get; set; }
        public double TimeDetProd { get; set; }
        public double TimeDetVehON { get; set; }
        public double MaxTimeStopOn { get; set; }
        public double Kmconsumidos { get; set; }
        public double CombConsumidos { get; set; }

        public string MaxSpeed_Title { get; set; }
        public string SumDistTot_Title { get; set; }
        public string TimeProd_Title { get; set; }
        public string TimeRecoProd_Title { get; set; }
        public string TimeDetProd_Title { get; set; }
        public string TimeDetVehON_Title { get; set; }
        public string MaxTimeStopOn_Title { get; set; }
        public string Kmconsumidos_Title { get; set; }
        public string CombConsumidos_Title { get; set; }
    }
}
