using static Program2.Template;

namespace Program2 {
	internal class Program {
		public static void Main(params string[] args) {
			GetTemplate(); // 导入已识别的验证码图片 将它作为后续识别学习的模板
		}
	}
}