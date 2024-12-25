using System;
using System.Collections.Generic;
using System.Linq;
using ResumeParser.API.Model;
using UglyToad.PdfPig.Content;
using static ResumeParser.API.Model.Enums;

namespace ResumeParser.API.ResumeConfigurations
{
    public class LinkedinResumeConfigurations : ResumeConfigurationsBase
    {
        public const double RIGHT_FRAME_POS = 223.56;
        public const double NAME_FONT_SIZE = 26;
        public const double HEADER_FONT_SIZE = 15.75;
        public const double TITLE_FONT_SIZE = 12;
        public const double SUBTITLE_FONT_SIZE = 11.5;
        public const double TEXT_FONT_SIZE = 10.5;
        public const double LEFT_FRAME_HEADER_FONT_SIZE = 13;

        public LinkedinResumeConfigurations(Dictionary<int, List<Word>> words)
        {
            // var lines = GetLines(words);

            // var leftFrameLines = lines.Where(x => x.HorizontalPosition < RIGHT_FRAME_POS).ToList();
            // var rightFrameLines = lines.Where(x => x.HorizontalPosition > RIGHT_FRAME_POS).ToList();

            // CreateResumeBody(leftFrameLines)
            //    .BodyOptions(options => options
            //        .SetSectionTitleRules(rules =>
            //            rules.AddFontSizeRule(LEFT_FRAME_HEADER_FONT_SIZE))
            //        )
            //    .DeclareSection("İletişim Bilgileri", SectionType.ContactInfo, x => x
            //        .DeclareResumeData<PersonalInfo>(
            //            x => x.PhoneNumber,
            //            rules => rules.AddTextRule(text => text.EndsWith("(Mobile)") || text.EndsWith("(Work)") || text.EndsWith("(Home)"))),
            //            text => text.Split(" ")[0]
            //            )
            //        .DeclareResumeData<PersonalInfo>(
            //            x => x.Email,
            //            rules => rules.AddTextRule(text => text.Contains("@"))
            //            )
            //        .DeclareResumeData<PersonalInfo>(
            //            x => x.LinkedinProfile,
            //            rules => rules.AddTextRule(text => text.EndsWith("www.linkedin.com/in/borabilir")),
            //            text => text.Split(" ")[0]
            //            )
            //    )
            //    .DeclareSection("En Önemli Yetenekler", SectionType.Skills, x => x
            //        .DeclareResumeData<Skill>(
            //            x => x.SkillName,
            //            rules => rules.FontSize(TEXT_FONT_SIZE)
            //            )
            //    )
            //    .DeclareSection("Languages", SectionType.Languages, x => x
            //        .DeclareResumeData<Language>(
            //            x => x.LanguageName,
            //            rules => rules.FontSize(TEXT_FONT_SIZE),
            //            text => text.Split(" ")[0]
            //            )
            //    );

            // var rightFrameWords = words.Where(x => x.BoundingBox.Left >= RIGHT_FRAME_POS).ToList();
            // var rightFrameLines = GetLines(rightFrameWords);

            // CreateResumeBody(rightFrameLines)
            //     .BodyOptions(options => options
            //        .SetSectionTitleRules(rules =>
            //            rules.FontSize(HEADER_FONT_SIZE))
            //        //.SetStartProperty<Education>(x => x.SchoolName)
            //        //.SetStartProperty<Experience>(x => x.CompanyName)
            //        )
            //    .DeclareSection("Özet", SectionType.Summary, x => x
            //        .DeclareResumeData<PersonalInfo>(
            //            x => x.Summary,
            //            rules => rules.FontSize(TITLE_FONT_SIZE)
            //            )
            //        )
            //    .DeclareSection("Deneyim", SectionType.Experience, x => x
            //        .DeclareResumeData<Experience>(
            //            x => x.CompanyName,
            //            rules => rules.FontSize(TITLE_FONT_SIZE)
            //            )
            //        .DeclareResumeData<Experience>(
            //            x => x.JobTitle,
            //            rules => rules.FontSize(SUBTITLE_FONT_SIZE),
            //            lineTag: "JobTitle"
            //            )
            //        .DeclareResumeData<Experience>(
            //            x => x.CompanyStartDate,
            //            rules => rules.CompareTag("JobTitle", TargetLine.Previous),
            //            text => text.Split("-")[0],
            //            lineTag: "Date"
            //            )
            //        .DeclareResumeData<Experience>(
            //            x => x.CompanyEndDate,
            //            rules => rules.CompareTag("JobTitle", TargetLine.Previous),
            //            text => text.Split("-")[1],
            //            lineTag: "Date"
            //            )
            //        .DeclareResumeData<Experience>(
            //            x => x.CompanyLocation,
            //            rules => rules.AddCustomRule(x => x.Text.Contains("stanbul") || x.Text.Contains("Davutpaşa")),
            //            lineTag: "Location"
            //            )
            //        .DeclareResumeData<Experience>(
            //            x => x.JobDescription,
            //            rules => rules.AddCustomRule(x => x.Tag == "JobTitle" || x.Tag == "Location" || x.Tag == "Description", TargetLine.Previous),
            //            lineTag: "Description"
            //            )
            //      )
            //    .DeclareSection("Eğitim", SectionType.Education, x => x
            //        .DeclareResumeData<Education>(
            //            x => x.SchoolName,
            //            rules => rules.FontSize(TITLE_FONT_SIZE)
            //            )
            //        .DeclareResumeData<Education>(
            //            x => x.Degree,
            //            rules => rules
            //                .FontSize(10.5)
            //                .AddTextRule(x => x.Contains(",")),
            //            text => text.Split(",")[0]
            //            )
            //        .DeclareResumeData<Education>(
            //            x => x.FieldOfStudy,
            //            rules => rules
            //                .FontSize(10.5)
            //                .AddTextRule(x => x.Contains(",")),
            //            text => text.Split(",")[1]
            //            ));


        }
    }
}
