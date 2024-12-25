using System;
using System.Linq.Expressions;
using ResumeParser.API.Model;
using ResumeParser.API.Model.Resume;
using static ResumeParser.API.Model.Enums;

namespace ResumeParser.API.ResumeConfigurations
{
    public class ResumeBodyOptions
    {
        private ResumeBody _resumeBody;

        public ResumeBodyOptions(ResumeBody resumeBody)
        {
            _resumeBody = resumeBody;
        }

        public ResumeBodyOptions SetSectionTitleRules(Action<LineRuleBuilder> setupAction)
        {
            var lineRules = new LineRuleBuilder(_resumeBody.TitleRules);
            setupAction.Invoke(lineRules);
            return this;
        }

        public ResumeBodyOptions SetStartProperty<T>(Expression<Func<T, string>> property) where T : ResumeInfo
        {
            var expr = property.Body as MemberExpression;
            if (expr == null)
                throw new InvalidOperationException("Invalid property type");

            var className = expr.Member.ReflectedType;
            var propName = expr.Member.Name;

            string startProperty;

            _resumeBody.StartConditions.TryGetValue(className, out startProperty);

            if (startProperty == null)
                _resumeBody.StartConditions.Add(className, propName);
            else
                _resumeBody.StartConditions[className] = propName;

            return this;
        }

    }
}
