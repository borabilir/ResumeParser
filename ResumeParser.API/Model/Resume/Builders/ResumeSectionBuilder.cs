using System;
using ResumeParser.API.Model.Resume.Builders;
using static ResumeParser.API.Model.Enums;

namespace ResumeParser.API.Model.Resume
{
    public class ResumeSectionBuilder
    {
        private ResumeBody _resumeBody;

        public ResumeSectionBuilder(ResumeBody resumeBody)
        {
            _resumeBody = resumeBody;
        }

        public ResumeSectionBuilder DeclareSection(string keyword, SectionType sectionType, Action<ResumeDataBuilder> dataSetupAction)
        {
            var section = new ResumeSection(keyword, sectionType);

            //var lineRules = new LineRuleBuilder(section.LineRules);
            //ruleSetupAction.Invoke(lineRules);

            var builder = new ResumeDataBuilder(section);
            dataSetupAction.Invoke(builder);

            _resumeBody.Sections.Add(section);
            return this;
        }

    }
}
