using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfTools
{
    public class ShadowViewModel : PropertyChangedBase
    {
        private DropShadowEffect dropShadowEffect= new DropShadowEffect();
        public DropShadowEffect DropShadowEffect
        {
            get { return dropShadowEffect; }
            set
            {
                dropShadowEffect = value;
                OnPropertyChanged(() => DropShadowEffect);
            }
        }
        public ShadowViewModel()
        {
            DropShadowEffect.Opacity = 1;
            DropShadowEffect.ShadowDepth = 5;
            DropShadowEffect.Direction= 315;
            DropShadowEffect.BlurRadius = 10;
            DropShadowEffect.Color = Colors.Gray;
            DropShadowEffect.RenderingBias = System.Windows.Media.Effects.RenderingBias.Performance;

            ColorCheckCommand = new RelayCommand(ColorCheck);
        }

        private void ColorCheck(object obj)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.SelectedColor = DropShadowEffect.Color;
            if ((bool)colorDialog.ShowDialog())
            {
                //RectColorPicked.Fill = new SolidColorBrush(colorDialog.SelectedColor);
                DropShadowEffect.Color =colorDialog.SelectedColor;
            }
        }

        private ICommand colorCheckCommand { get; set; }
        public ICommand ColorCheckCommand
        {
            get
            {
                return colorCheckCommand;
            }
            set
            {
                colorCheckCommand = value;
            }
        }
    }


    public class DropShadowEffect : PropertyChangedBase
    {
        private double shadowDepth;
        private Color color;
        private double direction;
        private double opacity;
        private double blurRadius;
        private System.Windows.Media.Effects.RenderingBias renderingBias;

        private Brush brushColor;

        // 纹理下方投影的距离。 默认值为 5。
        public double ShadowDepth
        {
            get { return shadowDepth; }
            set
            {
                shadowDepth = value;
                OnPropertyChanged(() => shadowDepth);
            }
        }

        // 投影的颜色。 默认值为 System.Windows.Media.Colors.Black。
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged(() => Color);
                BrushColor = new SolidColorBrush(color);
            }
        }

        //以度为单位的投影的方向。 默认值为 315。
        public double Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                OnPropertyChanged(() => Direction);
            }
        }

        //投影的不透明度。 默认值为 1。
        public double Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                OnPropertyChanged(() => Opacity);
            }
        }

        // 一个值，该值指示阴影的半径模糊效果。 默认值为 5。
        public double BlurRadius
        {
            get { return blurRadius; }
            set
            {
                blurRadius = value;
                OnPropertyChanged(() => BlurRadius);
            }
        }

        //一个 System.Windows.Media.Effects.RenderingBias 值，该值指示系统在呈现投影时注重速度还是质量。 默认值为 System.Windows.Media.Effects.RenderingBias.Performance。
        public System.Windows.Media.Effects.RenderingBias RenderingBias
        {
            get { return renderingBias; }
            set
            {
                renderingBias = value;
                OnPropertyChanged(() => RenderingBias);
            }
        }

        //颜色的16进制字串
        public Brush BrushColor
        {
            get { return brushColor; }
            set
            {
                brushColor = value;
                OnPropertyChanged(() => BrushColor);
            }
        }

    }
}
