using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// Derleme hakkında Genel Bilgiler aşağıdakilerle denetlenir. 
// kontrol edilir. Derlemeyle ilişkilendirilen bilgiyi değiştirmek için
// bu öznitelik değerlerini değiştirin.
[assembly: AssemblyTitle("DonanımKontrol")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("DonanımKontrol")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// ComVisible yanlış olarak ayarlamak bu derlemedeki türleri görünmez hale getirir 
// COM bileşenleri için.  Bu derlemedeki bir türe erişmeniz gerekiyorsa 
// şu türde ComVisible özniteliğini true olarak ayarlayın.
[assembly: ComVisible(false)]

//Yerelleştirilebilir uygulamalar oluşturmaya başlamak için .csproj 
//dosyanızdaki <UICulture>CultureYouAreCodingWith</UICulture> özelliğini
//bir <PropertyGroup> grubuna ayarlayın.  Örneğin, kaynak dosyalarınızda ABD ingilizcesi
//kullanıyorsanız, <UICulture> özelliğini en-US olarak ayarlayın. Sonra aşağıdaki
//NeutralResourceLanguage özniteliğinin açıklamasını kaldırın.  Proje dosyasındaki
//UICulture ayarıyla eşleşmesi için alt satırdaki "en-US"i güncelleyin.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //temaya özgü kaynak sözlüklerinin bulunduğu yer
    //(bir kaynak, sayfada veya 
    // uygulama kaynak sözlüklerinde bulunamazsa kullanılır)
    ResourceDictionaryLocation.SourceAssembly //genel kaynak sözlüğünün bulunduğu yer
    //(bir kaynak, sayfada veya 
    // uygulama veya herhangi bir temaya özgü kaynak sözlüklerinde bulunamazsa kullanılır)
)]


// Bir derlemenin sürüm bilgisi bahsedilen dört değerden meydana gelir:
//
//      Ana Sürüm
//      Alt Sürüm 
//      Yapı Sayısı
//      Düzeltme
//
// Bütün değerleri belirtebilir ya da Yapı ve Düzeltme Numaralarını varsayılan olarak seçebilirsiniz 
// Yapı Numaralarını varsayılan yapabilirsiniz:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
