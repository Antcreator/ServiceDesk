namespace ServiceDesk.Tickets.Service;

public class DocumentService(HttpClient http)
{
    public async Task UploadDocument(IFormFile document, Guid ticket)
    {
        await using var file = document.OpenReadStream();
        var multiFormData = new MultipartFormDataContent
        {
            { new StreamContent(file), "Attachment", document.FileName },
            { new StringContent(ticket.ToString()), "TicketId" }
        };

        var response = await http.PostAsync("/", multiFormData);

        response.EnsureSuccessStatusCode();
    }
}
