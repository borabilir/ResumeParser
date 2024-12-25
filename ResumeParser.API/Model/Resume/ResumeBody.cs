using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ResumeParser.API.ResumeConfigurations;
using static ResumeParser.API.Model.Enums;

namespace ResumeParser.API.Model.Resume
{
    public class ResumeBody
    {
        public ResumeBody(List<Line> lines)
        {
            Sections = new List<ResumeSection>();
            TitleRules = new Dictionary<Expression<Func<Line, bool>>, TargetLine>();
            Lines = lines;
        }

        public List<ResumeSection> Sections { get; set; }
        public List<Line> Lines { get; set; }

        public Dictionary<Type, string> StartConditions { get; set; }

        //Body'nin sectionlara bölünme kuralları
        public Dictionary<Expression<Func<Line, bool>>, TargetLine> TitleRules { get; set; }

        public ResumeSectionBuilder BodyOptions(Action<ResumeBodyOptions> setupAction)
        {
            var options = new ResumeBodyOptions(this);
            setupAction.Invoke(options);
            return new ResumeSectionBuilder(this);
        }

    }
}
