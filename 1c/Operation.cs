using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Workplace1c
{
    /// <summary>
    /// Операции, которые можно выполнять с базами.
    /// </summary>
    public class Operation
    {
        private readonly string platformVerison;
        private readonly Base currentBase;
        private readonly string serverOrFile;

        public Operation(Base currentBase, string platformVerison)
        {
            this.currentBase = currentBase;
            this.platformVerison = platformVerison;
            serverOrFile = currentBase.IsServer ? "/S" : "/F";
        }

        /// <summary>
        /// Выполняет команду 1С.
        /// </summary>
        public void StartProcess(string arguments)
        {
            try { Process.Start(platformVerison, arguments)?.WaitForExit(); }
            catch (Exception e) { System.Console.WriteLine(e.Message); } // TODO надо сообщать куда-то. Например создать класс для вывода.
        }

        /// <summary>
        /// Открывает базу в пользовательском режиме.
        /// </summary>
        public async Task OpenBaseUser()
        {
            string arguments =
                $"ENTERPRISE {serverOrFile} \"{currentBase.Folder}\" /N \"{currentBase.User}\"";

            await Task.Run(() => StartProcess(arguments));
        }

        /// <summary>
        /// Открывает базу в режиме конфигуратора.
        /// </summary>
        public async Task OpenBaseConfigurator()
        {
            string arguments =
                $"DESIGNER {serverOrFile} \"{currentBase.Folder}\" /N \"{currentBase.User}\"";

            await Task.Run(() => StartProcess(arguments));
        }

        /// <summary>
        /// Обновляет базу из хранилища 1С.
        /// </summary>
        public async Task UpdateRepository()
        {
            // Если есть пароль, тогда подставляем. С пустым паролем /ConfigurationRepositoryP работать не будет. 
            string pass = currentBase.RepositoryPass == String.Empty ? "" : $" /ConfigurationRepositoryP {currentBase.RepositoryPass}";
            string updateCommand = $"/ConfigurationRepositoryUpdateCfg " +
                                    $"/ConfigurationRepositoryF {currentBase.RepositoryPath} " +
                                    $"/ConfigurationRepositoryN {currentBase.RepositoryUser}{pass} /UpdateDBCfg";

            string arguments =
                $"DESIGNER {serverOrFile} {currentBase.Folder} /N \"{currentBase.User}\" {updateCommand}";

            await Task.Run(() => StartProcess(arguments));
        }

        /// <summary>
        /// Обновляет базу на поддержке новым файлом обновления .cfu.
        /// </summary>
        public async Task UpdateBaseWithDistributeFileCfu(string folderWithCf, string release)
        {
            string newCf = $"{folderWithCf}\\{release}\\1cv8.cfu";

            string arguments =
               $"DESIGNER {serverOrFile} {currentBase.Folder} /N \"{currentBase.User}\" /UpdateCfg {newCf} /UpdateDBCfg";

            await Task.Run(() => StartProcess(arguments));
        }

        /// <summary>
        /// Выгружает конфигурацию .cf в каталог.
        /// </summary>
        public async Task DumpConfiguration(string rootFolder, string fileName)
        {
            string arguments =
                $"DESIGNER {serverOrFile} {currentBase.Folder} /N \"{currentBase.User}\" /DumpCfg {rootFolder}\\{fileName}.cf";

            await Task.Run(() => StartProcess(arguments));
        }

        /// <summary>
        /// Делает файл поставки.
        /// </summary>
        public async Task CreateUpdateFile(string folderWithCf)
        {
            // Если есть пароль, тогда подставляем. С пустым паролем /ConfigurationRepositoryP работать не будет. 
            string pass = currentBase.RepositoryPass == string.Empty ? "" : $" /ConfigurationRepositoryP {currentBase.RepositoryPass}";
            string repository = $"/ConfigurationRepositoryUpdateCfg " +
                                    $"/ConfigurationRepositoryF {currentBase.RepositoryPath} " +
                                    $"/ConfigurationRepositoryN {currentBase.RepositoryUser}{pass}";

            string newCf = $"{folderWithCf}";

            string arguments =
                $"DESIGNER {serverOrFile} {currentBase.Folder} /N \"{currentBase.User}\" {repository} /CreateDistributionFiles -cffile " +
                $"{newCf}";

            await Task.Run(() => StartProcess(arguments));
        }

        /// <summary>
        /// Выгружает базу в файл .dt
        /// </summary>
        public async Task DumpBase(string rootFolder, string fileName)
        {
            string arguments =
                $"DESIGNER {serverOrFile} {currentBase.Folder} /N \"{currentBase.User}\" /DumpIB {rootFolder}\\{fileName}.dt";

            await Task.Run(() => StartProcess(arguments));
        }

        #region OLD

        // /// <summary>
        // /// Загружает базу с диска в базу 1С
        // /// </summary>
        // /// <param name="rootFolder">Каталог, где лежит база</param>
        // /// <param name="loadBaseName">Имя загружаемого файла ИБ</param>
        // /// <returns>Ошибка</returns>
        // Exception RestoreBase(string rootFolder, string loadBaseName)
        // {
        //     string arguments =
        //         $"DESIGNER {ServerFile(Convert.ToBoolean(currentBase.IsServer))} {currentBase.Folder} /N \"{currentBase.User}\" /RestoreIB {rootFolder}\\{loadBaseName} /UpdateDBCfg";

        //     return StartProcess(arguments);
        // }

        // /// <summary>
        // /// Загружает файл конфигурации в базу 1С
        // /// </summary>
        // /// <param name="rootFolder">Каталог, где лежит база</param>
        // /// <param name="configurationName">Имя загружаемого файла конфигурации</param>
        // /// <returns>Ошибка</returns>
        // Exception RestoreConfiguration(string rootFolder, string configurationName)
        // {
        //     string arguments =
        //         $"DESIGNER {ServerFile(Convert.ToBoolean(currentBase.IsServer))} {currentBase.Folder} /N \"{currentBase.User}\" /LoadCfg {rootFolder}\\{configurationName} /UpdateDBCfg";

        //     return StartProcess(arguments);
        // }

        // /// <summary>
        // /// Выгружает базу .dt в каталог.
        // /// </summary>
        // /// <param name="rootFolder">Каталог, куда выгружаем базу</param>
        // /// <param name="unloadBaseName">Имя выгружаемого файла ИБ</param>
        // /// <returns>Ошибка</returns>
        // Exception DumpBase(string rootFolder, string unloadBaseName)
        // {
        //     string arguments =
        //         $"DESIGNER {ServerFile(Convert.ToBoolean(currentBase.IsServer))} {currentBase.Folder} /N \"{currentBase.User}\" /DumpIB {rootFolder}\\{unloadBaseName}";

        //     return StartProcess(arguments);
        // }


        // /// <summary>
        // /// Создает файл поставки.
        // /// </summary>
        // /// <param name="folderWithCf">Путь до папки, где лежат папки с .cf разных релизов</param>
        // /// <param name="release">Релиз</param>
        // /// <param name="previousRelease">Предыдущие релизы, строка строго через запятую без пробелов</param>
        // /// <returns>Ошибка</returns>
        // Exception CreateUpdateFile(string folderWithCf, string release, string previousRelease)
        // {
        //     string repository = String.Empty;
        //     if (Convert.ToBoolean(currentBase.IsRepository))
        //     {
        //         // Если есть пароль, тогда подставляем. С пустым паролем /ConfigurationRepositoryP работать не будет. 
        //         string pass = currentBase.RepositoryPass == String.Empty ? "" : $" /ConfigurationRepositoryP {currentBase.RepositoryPass}";
        //         repository = $"/ConfigurationRepositoryF {currentBase.RepositoryPath} " +
        //                                 $"/ConfigurationRepositoryN {currentBase.RepositoryUser}{pass} /UpdateDBCfg";
        //     }

        //     string newCf = $"{folderWithCf}\\{release}\\1cv8.cf";
        //     string newCfu = newCf + "u";

        //     string[] arr = previousRelease.Split(',');
        //     string previousCf = "";
        //     foreach (var item in arr)
        //         previousCf += $"-f {folderWithCf}\\{item}\\1cv8.cf ";

        //     string arguments =
        //         $"DESIGNER {ServerFile(Convert.ToBoolean(currentBase.IsServer))} {currentBase.Folder} /N \"{currentBase.User}\" {repository} /CreateDistributionFiles -cffile " +
        //         $"{newCf} -cfufile {newCfu} {previousCf}";

        //     return StartProcess(arguments);
        // }

        // /// <summary>
        // /// Обновляет базу на поддержке новым файлом обновления.
        // /// </summary>
        // /// <param name="folderWithCf">Путь до папки, где лежат папки с .cf разных релизов</param>
        // /// <param name="release">Релиз</param>
        // /// <returns>Ошибка</returns>
        // Exception UpdateBaseWithDistributeFile(string folderWithCf, string release)
        // {
        //     string newCf = $"{folderWithCf}\\{release}\\1cv8.cf";

        //     string arguments =
        //        $"DESIGNER {ServerFile(Convert.ToBoolean(currentBase.IsServer))} {currentBase.Folder} /N \"{currentBase.User}\" /UpdateCfg {newCf} /UpdateDBCfg";

        //     return StartProcess(arguments);
        // }

        // /// <summary>
        // /// Создаёт сборку 1С из файла комплекта поставки .edf
        // /// </summary>
        // /// <param name="folderWithPackage">Путь до папки, где лежат файлы .edf</param>
        // /// <param name="release">Текущий релиз сборки</param>
        // /// <param name="folderForDistribution">Путь до папки, куда будет сделана сборка</param>
        // /// <param name="full">Опция - делать полную сборку или только обновление</param>
        // /// <returns>Ошибка</returns>
        // public Exception CreateDistribution(string folderWithPackage, string release, string folderForDistribution, bool full)
        // {
        //     string option = full == true ? "Полный" : "Обновление";

        //     string arguments =
        //         $"DESIGNER {ServerFile(Convert.ToBoolean(currentBase.IsServer))} {currentBase.Folder} /N \"{currentBase.User}\" /CreateDistributive " +
        //         $"{folderForDistribution} -File {folderWithPackage}\\КП_{release}.edf -Option {option}";

        //     return StartProcess(arguments);
        // }

        // /// <summary>
        // /// Удаляет связь с хранилищем 1С.
        // /// </summary>
        // /// <returns></returns>
        // public Exception UnbindBase()
        // {
        //     string arguments =
        //         $"DESIGNER {ServerFile(Convert.ToBoolean(currentBase.IsServer))} {currentBase.Folder} /N \"{currentBase.User}\" /ConfigurationRepositoryUnbindCfg -force";

        //     return StartProcess(arguments);
        // }

        #endregion

    }
}
