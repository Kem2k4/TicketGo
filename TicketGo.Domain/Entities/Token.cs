namespace TicketGo.Domain.Entities
{
    public partial class Token
    {
    public int Id { get; set; }
    public string Value { get; set; }
    public DateTime ExpiredAt { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; }
        
    }
}
