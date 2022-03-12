namespace Backend.WebApi.Models
{
  public class Filter
  {
    public string Column { get; set; }
    public string Operator { get; set; }
    public string Value { get; set; }

    public Filter() { }
    public Filter(string column, string @operator, string value)
    {
      Column = column;
      Operator = @operator;
      Value = value;
    }
  }
}
