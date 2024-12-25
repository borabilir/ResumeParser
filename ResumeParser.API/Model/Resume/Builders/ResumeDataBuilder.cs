using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static ResumeParser.API.Model.Enums;

namespace ResumeParser.API.Model.Resume.Builders
{
    public class ResumeDataBuilder
    {
        private ResumeSection _resumeSection;

        public ResumeDataBuilder(ResumeSection resumeSection)
        {
            _resumeSection = resumeSection;
        }

        public ResumeDataBuilder DeclareResumeData<T>(Expression<Func<T, string>> property, Action<LineRuleBuilder> ruleAction = null, Action<ReadExpressionBuilder> readAction = null, string lineTag = "") where T : ResumeInfo
        {
            var expr = property.Body as MemberExpression;
            if (expr == null)
                throw new InvalidOperationException("Invalid property type");
            var propertyName = expr.Member.Name;
            var classType = expr.Member.ReflectedType;

            var classProperty = new KeyValuePair<Type, string>(classType, propertyName);

            var resumeData = new ResumeData(classProperty, lineTag);

            if (ruleAction != null)
            {
                var lineRuleBuilder = new LineRuleBuilder(resumeData.LineRules);
                ruleAction.Invoke(lineRuleBuilder);
            }

            if (readAction != null)
            {
                var readExpressionBuilder = new ReadExpressionBuilder(resumeData);
                readAction.Invoke(readExpressionBuilder);
            }

            _resumeSection.DataCollection.Add(resumeData);
            return this;
        }
    }
}
