using System;
using System.Collections.Generic;
using System.Text;

namespace Theatre.Common
{
    public static class ValidationConstants
    {
        // Theatre
        public const int TheatreNameMinLength = 4;
        public const int TheatreNameMaxLength = 30;
        
        public const int TheatreNumberOfHallsMinValue = 1;
        public const int TheatreNumberOfHallsMaxValue = 10;

        public const int TheatreDirectorMinLength = 4;
        public const int TheatreDirectorMaxLength = 30;

        // Cast
        public const int CastFullNameMinLength = 4;
        public const int CastFullNameMaxLength = 30;

        public const int CastPhoneNumberMaxLength = 15;

        // Play
        public const int PlayTitleMinLength = 4;
        public const int PlayTitleMaxLength = 50;

        public const float PlayRatingMinValue = 0.00f;
        public const float PlayRatingMaxValue = 10.00f;

        public const int PlayDescriptionMaxLength = 700;

        public const int PlayScreenwriterMinLength = 4;
        public const int PlayScreenwriterMaxLength = 30;

        // Ticket
        public const decimal TicketPriceMinValue = 1.00m;
        public const decimal TicketPriceMaxValue = 100.00m;

        public const int TicketRowNumberMinValue = 1;
        public const int TicketRowNumberMaxValue = 10;
    }
}
