using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public abstract class BaseButton : MonoBehaviour
    {
        [SerializeField] protected Button _button;

        protected void Awake()
        {
            if (this._button != null) return;
            this._button = GetComponent<Button>();
        }

        protected void Start()
        {
            this._button.onClick.AddListener(this.Onclick);
        }
        
        protected abstract void Onclick();
    }
}