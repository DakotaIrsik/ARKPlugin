namespace ArkFury.Common.Models.Images.Interfaces
{
    public interface IWatermark
    {
        byte[] ImageData { get; set; }
        string Folder { get; }
    }
}
