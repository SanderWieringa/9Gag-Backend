using PostService.Dtos;
using System.Net;
using System.Text;

namespace AuthorizationServiceTests
{
    [TestClass]
    public class AuthorizationTests 
    {
        HttpClient _httpClient = new HttpClient();

        [TestMethod]
        public async Task CreatePost_RequiresAuthorization_ReturnsUnauthorizadWithoutToken()
        {
            // Arrange

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5106/api/Post");
            PostCreateDto postCreateDto = new PostCreateDto();
            postCreateDto.Title = "test";
            request.Content = new StringContent("", Encoding.UTF8, "multipart/form-data");

            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task CreatePost_RequiresAuthorization_ReturnsAuthorizedWithToken()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5106/api/Post");
            PostCreateDto postCreateDto = new PostCreateDto();
            postCreateDto.Title = "test";
            request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjbVE0em8wMEFoLzU0RTRIckgxOWp3UTdBc3BYWk9oVlJRTmtOcEhJRU11OFovSU0rdkdrcHdYbUV5Q1E2UmtUMiszaklqeDRyQTRldjhLZTFhWHR5QT09IiwianRpIjoiNDA3OTc4NWItY2FhZS00MDU3LWIxNWItOTU4MmZjMmYwNTY2IiwiYXVkIjpbIjg2MTA4NjA5NTYzLWczZWxyNmU0a2JxaXF2Njc3bnV1MWtsdHN1bDFzYjBqLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwiODYxMDg2MDk1NjMtZzNlbHI2ZTRrYnFpcXY2NzdudXUxa2x0c3VsMXNiMGouYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iXSwidXNlcklkIjoiNjQ3MjU5OGM1OTIzM2ZkMWY2YmVhY2Q5IiwiZXhwIjoxNzY0NDg2ODE5LCJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20ifQ.3jduLN-2gZR2VfxsANiqYxIWaEutKnLEV3Uh-eiFtsI");
            request.Content = new StringContent("", Encoding.UTF8, "multipart/form-data");

            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}