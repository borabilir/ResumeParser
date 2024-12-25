using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ResumeParser.API.Model;
using ResumeParser.API.Model.Resume;
using UglyToad.PdfPig.Content;
using static ResumeParser.API.Model.Enums;

namespace ResumeParser.API.ResumeConfigurations
{
    public abstract class ResumeConfigurationsBase
    {
        private const double PAGE_HEIGHT = 792;
        private const double MAX_LINE_HEIGHT = 20;
        private const double MAX_WORD_SPACING = 10;
        private const double BOTTOM_LINE_SENSITIVITY = 20;

        private List<ResumeBody> resumeBodies { get; set; } = new List<ResumeBody>();

        protected ResumeBody CreateResumeBody(List<Line> lines)
        {
            var resumeBody = new ResumeBody(lines);
            resumeBodies.Add(resumeBody);
            return resumeBody;
        }

        public ResumeModel Process()
        {
            var resumeModel = new ResumeModel();
            try
            {
                foreach (var resumeBody in resumeBodies)
                {
                    Dictionary<Type, ResumeInfo> currentObjects = CreateCurrentObjectCollection();
                    Dictionary<PropertyInfo, object> resumeModelObjects = GetResumeModelObjectCollection(resumeModel);

                    var bodyLines = resumeBody.Lines;
                    ResumeSection currentSection = null;

                    foreach (var key in currentObjects.Keys.ToList())
                        currentObjects[key] = (ResumeInfo)Activator.CreateInstance(key);

                    for (int i = 0; i < bodyLines.Count; i++)
                    {
                        var currentLine = bodyLines[i];

                        Console.WriteLine("Reading line: " + currentLine.Text);

                        if (currentLine.Text == "Bitiş Tarihi")
                        {
                            Console.WriteLine("here");
                        }

                        Console.WriteLine("Checking title rules");

                        List<string> sectionKeywords = resumeBody.Sections.Select(x => x.Keyword).ToList();
                        bool titleRuleResult = true;
                        if (resumeBody.TitleRules == null)
                            titleRuleResult = resumeBody.Sections.Any(x => x.Keyword == currentLine.Text);
                        else
                            titleRuleResult = CheckLineRules(bodyLines, currentLine, resumeBody.TitleRules);

                        //İlgili satır bir Section başlığıdır
                        if (titleRuleResult)
                        {

                            var newSection = resumeBody.Sections.FirstOrDefault(x => x.Keyword == currentLine.Text);
                            if (newSection != null)
                            {
                                Console.WriteLine("Title detected: " + newSection.Keyword);
                                currentSection = newSection;
                            }
                            continue;
                        }

                        Console.WriteLine("Iterating resume data");

                        foreach (var resumeData in currentSection.DataCollection)
                        {
                            bool lineRuleResult = CheckLineRules(bodyLines, currentLine, resumeData.LineRules);

                            //İlgili satır bilgileri resumeData ile eşleşiyor
                            if (lineRuleResult)
                            {
                                Console.WriteLine("Rule satisfied for: " + resumeData.ClassProperty);

                                string data = string.Empty;
                                var dataExp = resumeData.DataExpression.Key;
                                var targetLine = GetTargetLine(bodyLines, currentLine, resumeData.DataExpression.Value);

                                switch (resumeData.DataReadMethod)
                                {
                                    case ReadMethod.SingleLine:
                                        if (dataExp == null)
                                            data = currentLine.Text;
                                        else
                                            data = dataExp.Invoke(targetLine.Text);
                                        break;
                                    case ReadMethod.MultiLine:
                                        while (true)
                                        {
                                            bool conditions = CheckLineRules(bodyLines, targetLine, resumeData.ReadConditions);
                                            if (conditions)
                                            {
                                                data += " " + targetLine.Text;
                                            }
                                            else
                                            {
                                                data = data.Trim();
                                                break;
                                            }

                                            TargetLine direction = resumeData.DataReadDirection == ReadDirection.Default ? TargetLine.Next : TargetLine.Bottom;

                                            targetLine = GetTargetLine(bodyLines, targetLine, direction);
                                        }
                                        break;
                                }

                                Console.WriteLine("Data read: " + data);

                                if (string.IsNullOrEmpty(currentLine.Tag) && currentLine.Tag != resumeData.LineTag)
                                    currentLine.Tag = resumeData.LineTag;

                                var classProperty = resumeData.ClassProperty;
                                var classType = classProperty.Key;
                                var propertyInfo = GetPropertyInfo(classProperty);

                                //Güncellenecek nesne
                                ResumeInfo targetObject;
                                targetObject = currentObjects[classType];

                                //Güncellenecek nesne resumeModel içerisinde bir list mi?
                                var isGeneric = IsGenericProperty(classType);

                                if (isGeneric)
                                {
                                    var propertyValue = propertyInfo.GetValue(targetObject);

                                    //Eğer ilgili property value boş değil ise yeni bir nesne oluşturulup list içerisine eklenir
                                    if (propertyValue != null)
                                    {
                                        var genericPropertyInfo = GetGenericPropertyInfo(classType);
                                        object obj = resumeModelObjects.GetValueOrDefault(genericPropertyInfo);
                                        genericPropertyInfo.PropertyType.GetMethod("Add").Invoke(obj, new[] { targetObject });
                                        targetObject = (ResumeInfo)Activator.CreateInstance(classType);

                                        //Güncel nesneler içerisine yeni oluşturulan nesne setlenir.
                                        currentObjects[classType] = targetObject;
                                    }
                                }

                                propertyInfo.SetValue(targetObject, data);
                                Console.WriteLine("Data set");
                            }
                        }
                    }

                    Console.WriteLine("Setting current objects");
                    //Güncel nesneler resumeModel içerisine eklenir
                    foreach (var item in currentObjects)
                    {
                        var currentObjectType = item.Key;
                        var currentObject = item.Value;

                        var isGeneric = IsGenericProperty(item.Key);

                        if (isGeneric)
                        {
                            var genericPropertyInfo = GetGenericPropertyInfo(currentObjectType);
                            object obj = resumeModelObjects.GetValueOrDefault(genericPropertyInfo);
                            genericPropertyInfo.PropertyType.GetMethod("Add").Invoke(obj, new[] { currentObject });
                        }
                        else
                        {
                            var propertyInfo = resumeModel.GetType().GetProperties().FirstOrDefault(x => x.PropertyType == currentObjectType);
                            propertyInfo.SetValue(resumeModel, currentObject);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return resumeModel;
        }

        /// <summary>
        /// İlgili kural listesini mevcut satır için çalıştırır.
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="currentLine"></param>
        /// <param name="ruleList"></param>
        /// <returns></returns>
        private bool CheckLineRules(List<Line> lines, Line currentLine, Dictionary<Expression<Func<Line, bool>>, TargetLine> ruleList)
        {
            if (currentLine == null)
                return false;

            foreach (var titleRule in ruleList)
            {
                var titleRuleExpression = titleRule.Key;
                var targetLineEnum = titleRule.Value;

                Line targetLine = GetTargetLine(lines, currentLine, targetLineEnum);
                bool expressionResult;

                if (targetLine == null)
                    return false;
                else
                    expressionResult = titleRuleExpression.Compile().Invoke(targetLine);

                if (!expressionResult)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Verilen kelime listesine göre satırları oluşturur.
        /// </summary>
        /// <param name="wordsDictionary"></param>
        /// <returns></returns>
        protected List<Line> GetLines(Dictionary<int, List<Word>> wordsDictionary)
        {
            var lines = new List<Line>();
            //int verticalIndex = 0;

            foreach (var wordsKvp in wordsDictionary.OrderBy(x => x.Key))
            {
                var page = wordsKvp.Key;
                var words = wordsKvp.Value;

                words = words.OrderByDescending(x => x.BoundingBox.Bottom).ThenBy(x => x.BoundingBox.Left).ToList();

                var lastWord = words[0];
                Word currentWord;

                //verticalIndex++;
                var currentLine = CreateNewLine(words[0], page);
                lines.Add(currentLine);


                for (int i = 1; i < words.Count; i++)
                {
                    lastWord = words[i - 1];
                    currentWord = words[i];

                    if (currentWord.BoundingBox.Bottom == lastWord.BoundingBox.Bottom &&
                        currentWord.BoundingBox.Left - lastWord.BoundingBox.Right < MAX_WORD_SPACING)
                    {
                        currentLine.Text += " " + currentWord.Text;
                    }
                    else
                    {
                        //if (currentWord.BoundingBox.Bottom != lastWord.BoundingBox.Bottom)
                        //    verticalIndex++;

                        currentLine = CreateNewLine(currentWord, page);
                        lines.Add(currentLine);
                    }
                }
            }

            return lines.OrderBy(x => x.VerticalPosition).ThenBy(x => x.HorizontalPosition).ToList();
        }

        /// <summary>
        /// Verilen kelime ile başlayan bir satır oluşturur
        /// </summary>
        /// <param name="word"></param>
        /// <param name="verticalIndex"></param>
        /// <returns></returns>
        private Line CreateNewLine(Word word, int page)
        {
            Line line = new Line();
            line.HorizontalPosition = word.BoundingBox.Left;
            line.VerticalPosition = (page - 1) * PAGE_HEIGHT + PAGE_HEIGHT - word.BoundingBox.Bottom;
            line.FontName = word.Letters[0].FontName;
            line.FontSize = word.Letters[0].FontSize;
            line.Text = word.Text;

            return line;
        }

        /// <summary>
        /// Verilen tip resumeModel içerisinde bir List mi
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected bool IsGenericProperty(Type type)
        {
            foreach (PropertyInfo info in typeof(ResumeModel).GetProperties())
            {
                if (info.PropertyType.IsGenericType &&
                    info.PropertyType.GetGenericArguments()[0].Name == type.Name
                    )
                {
                    return true;
                }
            }

            return false;
        }

        protected PropertyInfo GetGenericPropertyInfo(Type type)
        {
            foreach (PropertyInfo info in typeof(ResumeModel).GetProperties())
            {
                if (info.PropertyType.IsGenericType &&
                    info.PropertyType.GetGenericArguments()[0].Name == type.Name
                    )
                {
                    return info;
                }
            }

            return null;
        }

        /// <summary>
        /// Process içerisinde güncellenen nesnelerin takibini yapabilmek için Dictionary oluşturur.
        /// </summary>
        /// <returns></returns>
        private Dictionary<Type, ResumeInfo> CreateCurrentObjectCollection()
        {
            var currentObjectCollection = new Dictionary<Type, ResumeInfo>();

            foreach (Type type in Assembly.GetAssembly(typeof(ResumeInfo)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ResumeInfo))))
            {
                currentObjectCollection.Add(type, null);
            }

            return currentObjectCollection;
        }

        /// <summary>
        /// Resume model propertyleri için nesneler yaratır.
        /// </summary>
        /// <param name="resumeModel"></param>
        /// <returns></returns>
        private Dictionary<PropertyInfo, object> GetResumeModelObjectCollection(ResumeModel resumeModel)
        {
            var collection = new Dictionary<PropertyInfo, object>();

            foreach (PropertyInfo info in typeof(ResumeModel).GetProperties())
            {
                var instance = Activator.CreateInstance(info.PropertyType);
                info.SetValue(resumeModel, instance);
                collection.Add(info, instance);
            }

            return collection;
        }

        /// <summary>
        /// Bir class içerisindeki ilgili propertyInfo'yu geri döner
        /// </summary>
        /// <param name="classProperty"></param>
        /// <returns></returns>
        private PropertyInfo GetPropertyInfo(KeyValuePair<Type, string> classProperty)
        {
            var type = classProperty.Key;
            var propertyName = classProperty.Value;

            foreach (var property in type.GetProperties())
            {
                if (property.Name == propertyName)
                {
                    return type.GetProperty(propertyName);
                }
            }

            return null;
        }

        /// <summary>
        /// Hedef satırı geri göner.
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="currentLine"></param>
        /// <param name="targetLine"></param>
        /// <returns></returns>
        private Line GetTargetLine(List<Line> lines, Line currentLine, TargetLine targetLine)
        {
            if (currentLine == null)
                return null;

            var currentIndex = lines.IndexOf(currentLine);

            switch (targetLine)
            {
                case TargetLine.Next:
                    return currentIndex + 1 < lines.Count ? lines[currentIndex + 1] : null;
                case TargetLine.Previous:
                    return currentIndex - 1 >= 0 ? lines[currentIndex - 1] : null;
                case TargetLine.Bottom:
                    return lines.Where(x => currentLine.VerticalPosition < x.VerticalPosition &&
                                            currentLine.VerticalPosition - x.VerticalPosition <= MAX_LINE_HEIGHT &&
                                            Math.Abs(x.HorizontalPosition - currentLine.HorizontalPosition) < BOTTOM_LINE_SENSITIVITY)
                                .OrderBy(x => x.VerticalPosition)
                                .ThenBy(x => Math.Abs(x.HorizontalPosition - currentLine.HorizontalPosition))
                                .FirstOrDefault();
                case TargetLine.Top:
                    return lines.Where(x => currentLine.VerticalPosition > x.VerticalPosition &&
                                            x.VerticalPosition - currentLine.VerticalPosition <= MAX_LINE_HEIGHT &&
                                            Math.Abs(x.HorizontalPosition - currentLine.HorizontalPosition) < BOTTOM_LINE_SENSITIVITY)
                                .OrderByDescending(x => x.VerticalPosition)
                                .ThenBy(x => Math.Abs(x.HorizontalPosition - currentLine.HorizontalPosition))
                                .FirstOrDefault();
            }

            return lines[currentIndex];
        }
    }
}
