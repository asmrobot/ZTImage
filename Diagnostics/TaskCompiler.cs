using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Diagnostics
{
    /// <summary>
    /// 动态编译字符串
    /// </summary>
    public class DynamicCompiler
    {   
        /// <summary>
        /// 从源码得到处理器
        /// </summary>
        /// <param name="source"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static T GetProcesser<T>(string source,params string[] assemblyName) where T:class
        {
            
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("swanter.Spider.Engine.dll");
            parameters.ReferencedAssemblies.Add("HtmlAgilityPack.dll");
            if (assemblyName != null && assemblyName.Length > 0)
            {
                foreach (var item in assemblyName)
                {
                    parameters.ReferencedAssemblies.Add(item);
                }
            }

            try
            {
                CompilerResults result = provider.CompileAssemblyFromSource(parameters, source);
                if (result.Errors.Count > 0)
                {
                    foreach (var item in result.Errors)
                    {
                        Console.WriteLine(item.ToString());
                    }
                    return null;
                }

                Assembly assembly = result.CompiledAssembly;
                Type processer= assembly.GetTypes().SingleOrDefault((type) => { return type.IsSubclassOf(typeof(T)); });
                if (processer == null)
                {
                    return null;
                }
                return assembly.CreateInstance(processer.FullName, true) as T;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 从源码文件得到处理器
        /// </summary>
        /// <param name="file"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static T GetProcesserFromFile<T>(string file,params string[] assemblyName) where T:class
        {
            string code = string.Empty;
            try
            {
                code = File.ReadAllText(file);
            }
            catch
            {
                return null;
            }
            return GetProcesser<T>(code, assemblyName);
        }
    }

}
