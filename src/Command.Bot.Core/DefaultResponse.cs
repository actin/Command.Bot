﻿using MargieBot.Models;

namespace Command.Bot.Core
{
    internal class DefaultResponse : ResponderBase
    {
        #region Implementation of IResponder

        public override BotMessage GetResponse(ResponseContext context)
        {
            return new BotMessage() { Text = "Sorry I don't know that command. Type *help* for command information." };
        }

        #endregion
    }
}