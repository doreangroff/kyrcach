namespace kyrcach;

public class Examination
{
    public int ExamId { get; set; }
    public string Driver { get; set; }
    public string MedCenter { get; set; }
    public string Doctor { get; set; }
    public string ExamType { get; set; }
    public enum ExamResult
    {
        Пройден,
        НеПройден,
        ТребуетсяПовторныйОсмотр
    }
    public string Conclusion { get; set; }
    public string Comment { get; set; }
}