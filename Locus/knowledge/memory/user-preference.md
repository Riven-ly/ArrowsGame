---
id: kd_builtin_memory_user_preference
injectMode: rule
aiEditMode: auto
maintenanceRules: |-
  - Record only long-term user preferences that stay stable across tasks
  - Prioritize language, reporting style, code style, taboos, and explicit requirements
  - Keep each entry short and limited to stable preferences or hard constraints
  - Keep the list within 20 items and merge similar preferences
  - Remove one-off arrangements, temporary phrasing, and unconfirmed inferences
---

用户偏好：除高风险、破坏性操作或关键需求歧义外，直接连续执行任务并交付最终结果，不要反复请求确认。
用户偏好：Unity UI 动画优先使用项目现有的 DOTween，不使用协程替代补间动画。
用户偏好：DOTween 动画默认不使用 SetUpdate；Unity 组件引用优先使用 Inspector/面板序列化引用，避免运行时 GetComponent。

