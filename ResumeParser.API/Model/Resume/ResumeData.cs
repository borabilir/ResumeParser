using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static ResumeParser.API.Model.Enums;

namespace ResumeParser.API.Model.Resume
{
    public class ResumeData
    {
        public ResumeData(KeyValuePair<Type, string> classProperty, string lineTag)
        {
            ClassProperty = classProperty;
            DataExpression = new KeyValuePair<Func<string, string>, TargetLine>();
            LineRules = new Dictionary<Expression<Func<Line, bool>>, TargetLine>();
            ReadConditions = new Dictionary<Expression<Func<Line, bool>>, TargetLine>();
            LineTag = lineTag;
        }

        //Hangi property için data elde edilecek
        public KeyValuePair<Type, string> ClassProperty { get; set; }

        //Satır için hangi koşullar sağlanırsa bu property olduğunu anlarım
        //TargetLine: hangi satır için bu kurallar kontrol edilecek
        public Dictionary<Expression<Func<Line, bool>>, TargetLine> LineRules { get; set; }

        //Datayı satırdan hangi şekilde elde ederim
        public KeyValuePair<Func<string, string>, TargetLine> DataExpression { get; set; }

        //Datayı okuma yolu
        //SingleLine: Sadece 
        public ReadMethod DataReadMethod { get; set; }

        //Multiline data hangi yönde okunacak
        public ReadDirection DataReadDirection { get; set; }

        //Hangi koşul sağlandığında okumayı bitiririm
        public Dictionary<Expression<Func<Line, bool>>, TargetLine> ReadConditions;

        public string LineTag { get; set; }
    }
}
