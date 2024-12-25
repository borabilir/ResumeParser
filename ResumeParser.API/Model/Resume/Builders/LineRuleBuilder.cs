using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static ResumeParser.API.Model.Enums;

namespace ResumeParser.API.Model.Resume
{
    public class LineRuleBuilder
    {
        private Dictionary<Expression<Func<Line, bool>>, TargetLine> _ruleList;

        public LineRuleBuilder(Dictionary<Expression<Func<Line, bool>>, TargetLine> ruleList)
        {
            _ruleList = ruleList;
        }

        public LineRuleBuilder AddCustomRule(Expression<Func<Line, bool>> rule, TargetLine targetLine = TargetLine.Current)
        {
            _ruleList.Add(rule, targetLine);
            return this;
        }

        public LineRuleBuilder AddFontSizeRule(double fontSize, TargetLine targetLine = TargetLine.Current)
        {
            _ruleList.Add(x => x.FontSize == fontSize, targetLine);
            return this;
        }

        public LineRuleBuilder AddKeywordRule(string keyword, TargetLine targetLine = TargetLine.Current)
        {
            _ruleList.Add(x => x.Text == keyword, targetLine);
            return this;
        }

        public LineRuleBuilder AddTagRule(string tag, TargetLine targetLine = TargetLine.Current)
        {
            _ruleList.Add(x => x.Tag == tag, targetLine);
            return this;
        }


        public LineRuleBuilder AddTextRule(Func<string, bool> textRule, TargetLine targetLine = TargetLine.Current)
        {
            _ruleList.Add(x => textRule.Invoke(x.Text), targetLine);
            return this;
        }

        public LineRuleBuilder CompareTag(string tag, TargetLine targetLine = TargetLine.Current)
        {
            _ruleList.Add(x => !string.IsNullOrEmpty(x.Tag) && x.Tag == tag, targetLine);
            return this;
        }

        public LineRuleBuilder FontColor()
        {
            return this;
        }

    }
}
