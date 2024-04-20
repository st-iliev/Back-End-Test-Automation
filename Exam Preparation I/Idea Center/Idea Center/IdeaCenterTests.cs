using Idea_Center.Models.Response;
using Idea_Center.Utilities;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;

namespace Idea_Center
{
    public class IdeaCenterTests : ApiHelper
    {
        [Test,Order(1)]
        public void Create_New_Idea()
        {
            // Arrange
            var IdeaRequest = new IdeaDTO
            {
                Title = GenerateIdeaTitle(),
                Description = "Test Idea Description"
            };

            //Act
            var request = new RestRequest("/api/Idea/Create");
            request.AddJsonBody(IdeaRequest);
            var response = client.Execute(request, Method.Post);
            var result = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            //Assert
            Assert.That(response.StatusCode , Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Message, Is.EqualTo("Successfully created!"));
        }
        [Test, Order(2)]
        public void Get_All_Ideas()
        {
            //Act
            var request = new RestRequest("/api/Idea/All");

            var response = client.Execute(request, Method.Get);
            var result = JsonSerializer.Deserialize<IdeaDTO[]>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Length, Is.GreaterThan(0));
            Assert.That(result[result.Length - 1].Id, Is.Not.Null);

            lastIdeaId = result[result.Length - 1].Id;
        }
        [Test, Order(3)]
        public void Edit_Last_Idea()
        {
            //Arrange
            var requestIdea = new IdeaDTO
            {
                Title = "Idea has been edited",
                Description = "Edited description"
            };

            //Act
            var request = new RestRequest($"/api/Idea/Edit");
            request.AddQueryParameter("ideaId", lastIdeaId);
            request.AddJsonBody(requestIdea);

            var response = client.Execute(request, Method.Put);
            var result = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Message, Is.EqualTo("Edited successfully"));
            Assert.That(result.Idea.Title, Is.EqualTo(requestIdea.Title));
            Assert.That(result.Idea.Description, Is.EqualTo(requestIdea.Description));
        }
        [Test, Order(4)]
        public void Delete_Edited_Idea()
        {
            //Act
            var request = new RestRequest($"/api/Idea/Delete");
            request.AddQueryParameter("ideaId", lastIdeaId);

            var response = client.Execute(request, Method.Delete);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.EqualTo("\"The idea is deleted!\""));
        }
        [Test, Order(5)]
        public void Create_An_Idea_Without_RequeredFields()
        {
            //Arrange
            var requestIdea = new IdeaDTO
            {
                Title = "",
                Description = ""
            };

            //Act
            var request = new RestRequest("/api/Idea/Create");
            request.AddJsonBody(requestIdea);
            var response = client.Execute(request, Method.Post);
            var result = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test, Order(6)]
        public void Edit_NonExist_Idea()
        {
            //Arrange
            var requestIdea = new IdeaDTO
            {
                Title = "Idea has been edited",
                Description = "Edited description"
            };
            //Act
            var request = new RestRequest($"/api/Idea/Edit");
            request.AddQueryParameter("ideaId", DateTime.Now.Ticks);
            request.AddJsonBody(requestIdea);

            var response = client.Execute(request, Method.Put);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("\"There is no such idea!\""));
        }
        [Test, Order(7)]
        public void Delete_NonExisting_Idea()
        {
            //Act
            var request = new RestRequest($"/api/Idea/Delete");
            request.AddQueryParameter("ideaId", DateTime.Now.Ticks);

            var response = client.Execute(request, Method.Delete);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("\"There is no such idea!\""));
        }
    }
}