using UnityEngine;
using UnityEngine.UI;

public class BloodImage : RawImage
{
    private Slider _BloodSlider;

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        //獲取血條
        if (_BloodSlider == null)
            _BloodSlider = transform.parent.parent.GetComponent<Slider>();

        //獲取血條的值
        if (_BloodSlider != null)
        {
            //刷新血條的顯示
            float value = _BloodSlider.value;
            uvRect = new Rect(0,0,value,1);
        }
    }
}
