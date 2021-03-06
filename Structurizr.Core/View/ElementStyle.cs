using System.Runtime.Serialization;

namespace Structurizr
{

    /// <summary>
    /// A definition of an element style.
    /// </summary>
    [DataContract]
    public class ElementStyle
    {
        
        /// <summary>
        /// The tag to which this element style applies.
        /// </summary>
        [DataMember(Name="tag", EmitDefaultValue=false)]
        public string Tag { get; set; }
  
        
        /// <summary>
        /// The width of the element, in pixels.
        /// </summary>
        [DataMember(Name="width", EmitDefaultValue=false)]
        public int? Width { get; set; }
  
        
        /// <summary>
        /// The height of the element, in pixels.
        /// </summary>
        [DataMember(Name="height", EmitDefaultValue=false)]
        public int? Height { get; set; }
  
        
        /// <summary>
        /// The background colour of the element, as a HTML RGB hex string (e.g.
        /// </summary>
        [DataMember(Name="background", EmitDefaultValue=false)]
        public string Background { get; set; }
  
        
        /// <summary>
        /// The foreground (text) colour of the element, as a HTML RGB hex string (e.g.
        /// </summary>
        [DataMember(Name="color", EmitDefaultValue=false)]
        public string Color { get; set; }
  
        
        /// <summary>
        /// The standard font size used to render text, in pixels.
        /// </summary>
        /// <value>The standard font size used to render text, in pixels.</value>
        [DataMember(Name="fontSize", EmitDefaultValue=false)]
        public int? FontSize { get; set; }
  
        
        /// <summary>
        /// The shape used to render the element.
        /// </summary>
        [DataMember(Name="shape", EmitDefaultValue=false)]
        public Shape Shape { get; set; }
  
        internal ElementStyle()
        {
        }

        public ElementStyle(string tag)
        {
            this.Tag = tag;
        }

    }
}
