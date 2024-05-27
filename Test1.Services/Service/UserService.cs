using Domain.Models;
using System.Xml.Linq;
using Test1.Common.Helpers;

namespace Test1.Services.Service
{
    public class UserService : IUserService
    {
        private readonly string _filePath;

        public UserService(SiteSettings siteSettings)
        {
            _filePath = siteSettings.FilePath;

            if (!File.Exists(_filePath))
            {
                new XDocument(new XElement("Users")).Save(_filePath);
            }
        }

        private void SaveDocument(XDocument xDocument)
        {
            try
            {
                xDocument.Save(_filePath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to save the document.", ex);
            }
        }

        public List<User> GetAllUsers()
        {
            XDocument xDocument = XDocument.Load(_filePath);
            List<User> users = xDocument.Descendants("User").Select(u => new User
            {
                Id = (int?)u.Element("Id") ?? 0,
                FirstName = (string)u.Element("FirstName") ?? string.Empty,
                LastName = (string)u.Element("LastName") ?? string.Empty,
                CellphoneNumber = (string)u.Element("CellphoneNumber") ?? string.Empty
            }).ToList();


            return users;
        }

        public void AddUser(User user)
        {
            XDocument xDocument = XDocument.Load(_filePath);

            XElement? existingUser = xDocument.Descendants("User")
                                    .FirstOrDefault(u => (int?)u.Element("Id") == user.Id);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User with the same ID already exists.");
            }

            user.Id = xDocument.Descendants("User").Max(u => (int?)u.Element("Id")) + 1 ?? 1;

            xDocument.Root?.Add(new XElement("User",
                new XElement("Id", user.Id),
                new XElement("FirstName", user.FirstName),
                new XElement("LastName", user.LastName),
                new XElement("CellphoneNumber", user.CellphoneNumber)));

            this.SaveDocument(xDocument);
        }

        public User GetUser(int id)
        {
            XDocument xDocument = XDocument.Load(_filePath);

            XElement? userElement = xDocument.Descendants("User").FirstOrDefault(u => int.Parse(u.Element("Id").Value) == id);

            if (userElement == null)
            {
                return null;
            }

            return new User
            {
                Id = int.Parse(userElement.Element("Id").Value),
                FirstName = userElement.Element("FirstName").Value,
                LastName = userElement.Element("LastName").Value,
                CellphoneNumber = userElement.Element("CellphoneNumber").Value
            };
        }

        public void UpdateUser(User user)
        {
            XDocument xDocument = XDocument.Load(_filePath);

            XElement? userElement = xDocument.Descendants("User").FirstOrDefault(u => int.Parse(u.Element("Id").Value) == user.Id);

            if (userElement == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            userElement.Element("FirstName").Value = user.FirstName;
            userElement.Element("LastName").Value = user.LastName;
            userElement.Element("CellphoneNumber").Value = user.CellphoneNumber;

            xDocument.Save(_filePath);
        }

        public void DeleteUser(int id)
        {
            XDocument xDocument = XDocument.Load(_filePath);

            XElement? userElement = xDocument.Descendants("User").FirstOrDefault(u => int.Parse(u.Element("Id").Value) == id);

            if (userElement != null)
            {
                userElement.Remove();
                xDocument.Save(_filePath);
            }
            else
            {
                throw new InvalidOperationException("User not found.");
            }
        }


    }
}