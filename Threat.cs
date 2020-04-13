using System;


namespace Homework_2_Csharp_Courses
{
    [Serializable()]
    public class Threat
    {
        
        public int Identificator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Objective { get; set; }
        public bool IsIntegrityBroken { get; set; }
        public bool IsAccessibilityBroken { get; set; }
        public bool IsConfodentialityBroken { get; set; }

        public Threat(int identificator, string name, string description, string source, string objective, bool isIntegrityBroken, bool isAccessibilityBroken, bool isConfodentialityBroken)
        {
            Identificator = identificator;
            Name = name;
            Description = description;
            Source = source;
            Objective = objective;
            IsIntegrityBroken = isIntegrityBroken;
            IsAccessibilityBroken = isAccessibilityBroken;
            IsConfodentialityBroken = isConfodentialityBroken;
        }

        public override string ToString()
        {
            string result = $"Идентификатор: {this.Identificator}" +
                $"\n\nНаименование: {this.Name}" +
                $"\n\nОписание: {this.Description}" +
                $"\n\nИсточник угрозы: {this.Source}" +
                $"\n\nОбъект воздействия: {this.Objective}" +
                $"\n\nНарушение конфиденциальности: {(this.IsConfodentialityBroken ? "Да" : "Нет")}" +
                $"\n\nНарушение доступности: {(this.IsAccessibilityBroken ? "Да" : "Нет")}" +
                $"\n\nНарушение целостности: {(this.IsIntegrityBroken ? "Да" : "Нет")}";
            return result;
        }
    }
}
