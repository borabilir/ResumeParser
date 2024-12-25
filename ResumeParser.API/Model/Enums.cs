using System;
namespace ResumeParser.API.Model
{
    public class Enums
    {
        public enum ResumeType
        {
            Linkedin,
            KariyerNet
        }

        public enum SectionType
        {
            ContactInfo,
            Summary,
            Experience,
            Skills,
            Languages,
            Education
        }

        public enum TargetLine
        {
            Current,
            Previous,
            Next,
            Top,
            Bottom
        }

        public enum ReadDirection
        {
            Default,
            Bottom
        }

        public enum ReadMethod
        {
            SingleLine,
            MultiLine
        }

    }
}
