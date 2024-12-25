using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Content;
using System.Linq;
using static ResumeParser.API.Model.Enums;
using ResumeParser.API.Model;
using ResumeParser.API.Model.Resume.Builders;

namespace ResumeParser.API.ResumeConfigurations
{
    public class KariyerNetResumeConfigurations : ResumeConfigurationsBase
    {
        private const double HEADER_FONT_SIZE = 20;
        private const double TITLE_FONT_SIZE = 12;
        private const double TEXT_FONT_SIZE = 14;

        public KariyerNetResumeConfigurations(Dictionary<int, List<Word>> words)
        {
            var lines = GetLines(words);

            Action<ReadExpressionBuilder> defaultReadAction = x => x.ReadWhile(rules => rules.AddFontSizeRule(TEXT_FONT_SIZE), startLine: TargetLine.Bottom, readDirection: ReadDirection.Bottom);

            CreateResumeBody(lines)
                .BodyOptions(options => options
                    .SetSectionTitleRules(rules =>
                        rules.AddFontSizeRule(HEADER_FONT_SIZE))
                    )
                .DeclareSection("İletişim Bilgileri", SectionType.ContactInfo, x => x
                    .DeclareResumeData<PersonalInfo>(
                        property: x => x.Email,
                        ruleAction: rules => rules.AddKeywordRule("E-Posta Adresi").AddFontSizeRule(TITLE_FONT_SIZE),
                        readAction: defaultReadAction
                        )
                   .DeclareResumeData<PersonalInfo>(
                        property: x => x.PhoneNumber,
                        ruleAction: rules => rules.AddKeywordRule("Telefon").AddFontSizeRule(TITLE_FONT_SIZE),
                        readAction: defaultReadAction
                        )
                   .DeclareResumeData<PersonalInfo>(
                        property: x => x.Address,
                        ruleAction: rules => rules.AddKeywordRule("Adres").AddFontSizeRule(TITLE_FONT_SIZE),
                        readAction: defaultReadAction
                        )
                )
                .DeclareSection("İş Deneyimleri", SectionType.Experience, x => x
                       .DeclareResumeData<Experience>(
                           property: x => x.JobTitle,
                           ruleAction: rules => rules.AddKeywordRule("Pozisyon").AddFontSizeRule(TITLE_FONT_SIZE),
                           readAction: defaultReadAction
                           )
                      .DeclareResumeData<Experience>(
                           property: x => x.CompanyName,
                           ruleAction: rules => rules.AddKeywordRule("Firma Adı").AddFontSizeRule(TITLE_FONT_SIZE),
                           readAction: defaultReadAction
                           )
                      .DeclareResumeData<Experience>(
                           property: x => x.Location,
                           ruleAction: rules => rules.AddKeywordRule("Şehir").AddFontSizeRule(TITLE_FONT_SIZE),
                           readAction: defaultReadAction
                           )
                       .DeclareResumeData<Experience>(
                           property: x => x.StartDate,
                           ruleAction: rules => rules.AddKeywordRule("Başlangıç Tarihi").AddFontSizeRule(TITLE_FONT_SIZE),
                           readAction: defaultReadAction
                           )
                       .DeclareResumeData<Experience>(
                           property: x => x.EndDate,
                           ruleAction: rules => rules.AddKeywordRule("Bitiş Tarihi").AddFontSizeRule(TITLE_FONT_SIZE),
                           readAction: defaultReadAction
                           )
                       .DeclareResumeData<Experience>(
                           property: x => x.Description,
                           ruleAction: rules => rules.AddKeywordRule("İş Tanımı").AddFontSizeRule(TITLE_FONT_SIZE),
                           readAction: defaultReadAction
                           )
                   )
                .DeclareSection("Eğitim Bilgileri", SectionType.Education, x => x
                    .DeclareResumeData<Education>(
                        property: x => x.SchoolName,
                        ruleAction: rules => rules.AddKeywordRule("Üniversite").AddFontSizeRule(TITLE_FONT_SIZE),
                        readAction: defaultReadAction
                        )
                   .DeclareResumeData<Education>(
                        property: x => x.FieldOfStudy,
                        ruleAction: rules => rules.AddKeywordRule("Bölüm").AddFontSizeRule(TITLE_FONT_SIZE),
                        readAction: defaultReadAction
                        )
                   .DeclareResumeData<Education>(
                        property: x => x.SchoolStartDate,
                        ruleAction: rules => rules.AddKeywordRule("Başlangıç Tarihi").AddFontSizeRule(TITLE_FONT_SIZE),
                        readAction: defaultReadAction
                        )
                   .DeclareResumeData<Education>(
                        property: x => x.SchoolEndDate,
                        ruleAction: rules => rules.AddKeywordRule("Bitiş Tarihi", TargetLine.Top).AddFontSizeRule(TITLE_FONT_SIZE, TargetLine.Top),
                        lineTag: "EndDate"
                        )
                   .DeclareResumeData<Education>(
                        property: x => x.Degree,
                        ruleAction: rules => rules.AddTagRule("EndDate", TargetLine.Previous)
                        )
                )
                .DeclareSection("Yabancı Dil", SectionType.Languages, x => x
                       .DeclareResumeData<Language>(
                           property: x => x.Name,
                           ruleAction: rules => rules.AddKeywordRule("Dil", TargetLine.Top).AddFontSizeRule(TEXT_FONT_SIZE),
                           lineTag: "Language"
                           )
                      .DeclareResumeData<Language>(
                           property: x => x.Level,
                           ruleAction: rules => rules.AddTagRule("Language", TargetLine.Top)
                           )
                   )
                .DeclareSection("Yetkinlikler", SectionType.Skills, x => x
                       .DeclareResumeData<Skill>(
                           property: x => x.Name,
                           ruleAction: rules => rules.AddKeywordRule("Bilgisayar Bilgileri", TargetLine.Previous).AddFontSizeRule(TEXT_FONT_SIZE),
                           lineTag: "Language"
                           )
                        .DeclareResumeData<Certificate>(
                           property: x => x.Name,
                           ruleAction: rules => rules.AddKeywordRule("Sertifika Adı").AddFontSizeRule(TITLE_FONT_SIZE),
                           readAction: defaultReadAction
                           )
                        .DeclareResumeData<Certificate>(
                           property: x => x.Date,
                           ruleAction: rules => rules.AddKeywordRule("Sertifika Tarihi").AddFontSizeRule(TITLE_FONT_SIZE),
                           readAction: defaultReadAction
                           )
                        .DeclareResumeData<Certificate>(
                           property: x => x.CompanyName,
                           ruleAction: rules => rules.AddKeywordRule("Alındığı Kurum").AddFontSizeRule(TITLE_FONT_SIZE),
                           readAction: defaultReadAction
                           )
                        .DeclareResumeData<Certificate>(
                           property: x => x.Description,
                           ruleAction: rules => rules.AddKeywordRule("Açıklama").AddFontSizeRule(TITLE_FONT_SIZE),
                           readAction: defaultReadAction
                           )
                   );

        }
    }
}
