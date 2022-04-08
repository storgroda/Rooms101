namespace Rooms101
{
    [Serializable]
    public class MeetingConflictException : Exception
    {
        public MeetingConflictException() { }

        public MeetingConflictException(string message)
            : base(message) { }

        public MeetingConflictException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class NotYourMeetingException : Exception
    {
        public NotYourMeetingException() { }

        public NotYourMeetingException(string message)
            : base(message) { }

        public NotYourMeetingException(string message, Exception inner)
            : base(message, inner) { }
    }
}
