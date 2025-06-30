namespace ArkFury.Entities.ElasticSearch
{
    public class ElasticPurchase : BaseElastic
    {
      public string ReferenceNumber{ get; set; }
        public string ItemNumber { get; set; }
        public double Ammount { get; set; }
    }
}
