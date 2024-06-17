namespace ServiceDesk.Util;

public class HashSettings
{
    public const string Section = "Hash";
    public required string Salt { get; set; }
    public required int Iteration { get; set; }
}
