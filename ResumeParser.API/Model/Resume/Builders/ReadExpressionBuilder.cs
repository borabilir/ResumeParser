using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static ResumeParser.API.Model.Enums;

namespace ResumeParser.API.Model.Resume.Builders
{
    public class ReadExpressionBuilder
    {
        private ResumeData _resumeData;

        public ReadExpressionBuilder(ResumeData resumeData)
        {
            _resumeData = resumeData;
        }

        public void Read(Func<string, string> readFunc, TargetLine targetLine = TargetLine.Current)
        {
            _resumeData.DataReadMethod = ReadMethod.SingleLine;
            _resumeData.DataExpression = new KeyValuePair<Func<string, string>, TargetLine>(readFunc, targetLine);
        }

        public void Read(TargetLine targetLine = TargetLine.Current)
        {
            _resumeData.DataReadMethod = ReadMethod.SingleLine;
            _resumeData.DataExpression = new KeyValuePair<Func<string, string>, TargetLine>(x => x, targetLine);
        }

        public void ReadWhile(Func<string, string> readFunc, Action<LineRuleBuilder> readConditionsAction, TargetLine startLine = TargetLine.Current, ReadDirection readDirection = ReadDirection.Default)
        {
            _resumeData.DataReadMethod = ReadMethod.MultiLine;
            _resumeData.DataExpression = new KeyValuePair<Func<string, string>, TargetLine>(readFunc, startLine);
            _resumeData.DataReadDirection = readDirection;
            var lineRuleBuilder = new LineRuleBuilder(_resumeData.ReadConditions);
            readConditionsAction.Invoke(lineRuleBuilder);
        }

        public void ReadWhile(Action<LineRuleBuilder> readConditionsAction, TargetLine startLine = TargetLine.Current, ReadDirection readDirection = ReadDirection.Default)
        {
            _resumeData.DataReadMethod = ReadMethod.MultiLine;
            _resumeData.DataExpression = new KeyValuePair<Func<string, string>, TargetLine>(x => x, startLine);
            _resumeData.DataReadDirection = readDirection;
            var lineRuleBuilder = new LineRuleBuilder(_resumeData.ReadConditions);
            readConditionsAction.Invoke(lineRuleBuilder);
        }
    }
}
