using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] public RectTransform joystickArea;  // 親要素（背景円）
    [SerializeField] public RectTransform joystick;      // 子要素（つまみ）

    private Vector2 inputVector;

    /// ポインターを押した時
    public void OnPointerDown(PointerEventData eventData)
    {
        // 親要素のローカル座標を基準にタップ位置を取得
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickArea, eventData.position, eventData.pressEventCamera, out localPoint);

        // 親要素の範囲内に制限
        float radius = joystickArea.sizeDelta.x / 2f;
        if (localPoint.magnitude > radius)
        {
            localPoint = localPoint.normalized * radius;  // 親要素の半径内に収める
        }

        // 子要素（つまみ）をタップした位置に移動
        joystick.anchoredPosition = localPoint;

        // 入力ベクトルを正規化して格納
        inputVector = localPoint / radius;

        // ジョイスティックを回転させる
        RotateJoystick();
    }

    /// ポインターをドラッグしている時
    public void OnDrag(PointerEventData eventData)
    {
        // タッチ位置を取得し、親要素からのローカル座標に変換
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickArea, eventData.position, eventData.pressEventCamera, out localPoint);

        // ローカル座標を親要素の半径内に制限
        float radius = joystickArea.sizeDelta.x / 2f;
        if (localPoint.magnitude > radius)
        {
            localPoint = localPoint.normalized * radius;
        }

        // 子要素（つまみ）の位置を更新
        joystick.anchoredPosition = localPoint;

        // 入力ベクトルを正規化して格納
        inputVector = localPoint / radius;

        // ジョイスティックを回転させる
        RotateJoystick();
    }

    /// ポインターを離した時
    public void OnPointerUp(PointerEventData eventData)
    {
        // 子要素（つまみ）を初期位置に戻す
        joystick.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;  // 入力リセット
        // 回転をリセット
        joystick.rotation = Quaternion.Euler(0, 0, 0);
    }

    /// 水平方向の入力を取得
    public float Horizontal()
    {
        return inputVector.x;
    }

    /// 垂直方向の入力を取得
    public float Vertical()
    {
        return inputVector.y;
    }

    /// ジョイスティックを回転させる
    private void RotateJoystick()
    {
        // 入力ベクトルを基に角度を計算
        float angle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;

        // 90度のオフセットを加え、Z軸回転を適用
        joystick.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
