namespace uCondition.Models
{
    public class EditableProperty
    {
        /// <summary>
        /// Alias of the editable property.  Use this to access the value in code.
        /// </summary>
        public string Alias
        {
            get; set;
        }
    
        /// <summary>
        /// Data Type Alias or core view
        /// </summary>
        public string Control
        {
            get; set;
        }

        /// <summary>
        /// Name of the editable property.  Displayed to the CMS Editor
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// This helps render the value selected correctly within the CMS.  
        /// If datatype is a list of prevalues (e.g. dropdown), select PreValues.
        /// If datatype is a boolean (1 or 0) choose Boolean.
        /// If datatype value doesn't need to be shown, choose hidden.
        /// Else, no need to set this.
        /// </summary>
        public EditablePropertyDisplayMode ValueType { get; set; }

        /// <summary>
        /// Constructs a new editable property for editors to configure the predicate
        /// </summary>
        public EditableProperty()
        {
            Name = "";
            Alias = "";
            Control = "";
            ValueType = EditablePropertyDisplayMode.Standard;
        }

        /// <summary>
        /// Constructs a new editable property for editors to configure the predicate
        /// </summary>
        /// <param name="name">The display name that content editors see</param>
        /// <param name="alias">The alias of the field - use this to access the value</param>
        /// <param name="control">The type of control - can reference a data type alias or core view</param>
        public EditableProperty(string name, string alias, string control)
        {
            Name = name;
            Alias = alias;
            Control = control;
            ValueType = EditablePropertyDisplayMode.Standard;
        }

        /// <summary>
        /// Constructs a new editable property for editors to configure the predicate
        /// </summary>
        /// <param name="name">The display name that content editors see</param>
        /// <param name="alias">The alias of the field - use this to access the value</param>
        /// <param name="control">The type of control - can reference a data type alias or core view</param>
        /// <param name="valueType">
        /// This helps render the value selected correctly within the CMS.  
        /// If datatype is a list of prevalues (e.g. dropdown), select PreValues.
        /// </param>
        public EditableProperty(string name, string alias, string control, EditablePropertyDisplayMode valueType)
        {
            Name = name;
            Alias = alias;
            Control = control;
            ValueType = valueType;
        }
    }
}
