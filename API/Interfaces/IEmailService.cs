﻿namespace AsparagusN.Interfaces;

public interface IEmailService
{
    public Task<bool> SendEmailAsync(string email, string subject, string htmlMessage);
}