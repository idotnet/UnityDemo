using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using Unity;

namespace UnityDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ContainerCode();
            //ContainerConfiguration();
        }

        /// <summary>
        /// 代码注入
        /// </summary>
        public static void ContainerCode()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IProduct, Milk>();
            // 默认注册 无命名，如果后面还有默认注册会覆盖前面的

            container.RegisterType<IProduct, Sugar>("Sugar"); //命名注册

            IProduct _product = container.Resolve<IProduct>(); //解析默认对象
            _product.ClassName = _product.GetType().ToString();
            _product.ShowInfo();

            IProduct _sugar = container.Resolve<IProduct>("Sugar"); //指定命名解析对象
            _sugar.ClassName = _sugar.GetType().ToString();
            _sugar.ShowInfo();

            IEnumerable<IProduct> classList = container.ResolveAll<IProduct>();
            //获取容器中所有 IProduct 注册的已命名对象

            foreach (var item in classList)
            {
                item.ClassName = item.GetType().ToString();
                item.ShowInfo();
            } 
        }

        /// <summary>
        /// 配置文件注入
        /// </summary>
        public static void ContainerConfiguration()
        {
            IUnityContainer container = new UnityContainer();
            container.LoadConfiguration("MyContainer");
            UnityConfigurationSection section
              = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");//获取指定名称的配置节   
            section.Configure(container, "MyContainer");//获取特定配置节下已命名的配置节<container name='MyContainer'>下的配置信息

            IProduct classInfo = container.Resolve<IProduct>("Sugar");
            classInfo.ClassName = classInfo.GetType().ToString();
            classInfo.ShowInfo();
        }
    }

    /// <summary>
    /// 商品
    /// </summary>
    public interface IProduct
    {
        string ClassName { get; set; }
        void ShowInfo();
    }

    /// <summary>
    /// 牛奶
    /// </summary>
    public class Milk:IProduct
    {
        public string ClassName { get; set; }
        public void ShowInfo()
        {
            Console.WriteLine("牛奶：{0}", ClassName);
        }
    }

    /// <summary>
    /// 糖
    /// </summary>
    public class Sugar:IProduct
    {
        public string ClassName { get; set; }
        public void ShowInfo()
        {
            Console.WriteLine("糖：{0}", ClassName);
        }
    }
}
