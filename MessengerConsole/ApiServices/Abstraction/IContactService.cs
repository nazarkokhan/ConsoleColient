﻿using System.Threading.Tasks;
using MessengerApp.Core.DTO.Contact;
using MessengerConsole.DTO;

namespace MessengerConsole.ApiServices.Abstraction
{
    public interface IContactService
    {
        Task<Pager<ContactDto>> GetContactsPageAsync(
            int userId, string search, int page, int items);

        Task<ContactDto> GetContactAsync(
            int userId, int userContactId);

        Task<ContactDto> CreateContactAsync(
            int userId, CreateContactDto createContactDto);

        Task<ContactDto> EditContactAsync(
            int userId, EditContactDto editContactDto);

        Task DeleteContactAsync(
            int userId, int contactId);
    }
}