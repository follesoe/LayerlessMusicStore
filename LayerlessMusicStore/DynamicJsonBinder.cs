using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace LayerlessMusicStore
{
    public class DynamicJsonBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var inputStream = controllerContext.HttpContext.Request.InputStream;
            inputStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            string bodyText = reader.ReadToEnd();
            reader.Close();

            return String.IsNullOrEmpty(bodyText) ? null : JsonConvert.DeserializeObject(bodyText);
        }
    }

    public class DynamicJsonAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new DynamicJsonBinder();
        }
    }
}