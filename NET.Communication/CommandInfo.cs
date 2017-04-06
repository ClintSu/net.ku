using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Communication
{
    [Serializable]
    public class CommandInfo
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="commandaction">类别</param>
        /// <param name="key">命令</param>
        /// <param name="command">命令内容</param>
        /// <param name="method">访问方式</param>
        /// <param name="description">描述</param>
        public CommandInfo(EnumCommandAction commandaction, string key, string command, string method, string description)
        {
            _commandAction = commandaction;
            _key = key;
            _command = command;
            _method = method;
            _description = description;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="commandaction">类别</param>
        /// <param name="key">命令</param>
        /// <param name="command">命令内容</param>
        /// <param name="method">访问方式</param>
        /// <param name="description">描述</param>
        /// <param name="version">版本</param>
        public CommandInfo(EnumCommandAction commandaction, string key, string command, string method,
            string description, string version)
            : this(commandaction, key, command, method, description)
        {
            _version = version;
        }

        #region 属性
        private readonly string _key;
        private readonly string _version = "1.0";
        private readonly EnumCommandAction _commandAction;
        private readonly string _command;
        private readonly string _description;
        private readonly string _method;
        private string[] _args;
        private object _model;

        /// <summary>
        /// 命令Key
        /// </summary>
        public string Key => _key;
        /// <summary>
        /// 命令类型
        /// </summary>
        public EnumCommandAction CommandAction => _commandAction;
        /// <summary>
        /// 命令值
        /// </summary>
        public string Command => _command;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description => _description;
        /// <summary>
        /// 命令方式（Get/Post）
        /// </summary>
        public string Method => _method;
        /// <summary>
        /// 版本
        /// </summary>
        public string Version => _version;
        /// <summary>
        /// 参数
        /// </summary>
        public string[] Args => _args;
        /// <summary>
        /// 内容（Body）
        /// </summary>
        public object Model => _model;

        #endregion

        /// <summary>
        /// 访问初始化
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CommandInfo SetValue(string[] args)
        {
            this._args = args;
            return this;
        }
        /// <summary>
        /// 访问初始化
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CommandInfo SetValue(string[] args, object model)
        {
            this._args = args;
            this._model = model;
            return this;
        }
    }
    public enum EnumCommandAction
    {
        None = 0,
        Read = 1,
        Write = 2
    }
}
