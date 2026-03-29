# 🧠 AnatoLens – AR Anatomy Learning App

AnatoLens is an **Augmented Reality (AR) based anatomy learning application** built using Unity and Vuforia.
It allows users to visualize 3D anatomical models, interact with them, and learn through labels, descriptions, and quizzes.

---

## 🚀 Current Features (Phase 1 Completed)

### 🧊 AR Model Visualization

* Displays a 3D anatomical model (currently **Brain**) using image tracking
* Model appears anchored on a real-world marker
* Supports:

  * 🔄 Rotation (touch drag)
  * 🔍 Zoom (pinch gesture)
  * 📌 Hold / Release model

---

### 🏷️ Interactive Labeling System

* Different parts of the brain are labeled
* Labels appear dynamically on the model
* Clicking a label:

  * Shows information about that part
  * Triggers quiz availability

---

### 📚 Info Panel (Per Part)

* Displays:

  * Part name
  * Short description
* Designed to be extendable for:

  * Images
  * Detailed explanations

---

### 🧪 Quiz System

* Each labeled part can trigger a quiz
* Currently:

  * Basic quiz structure implemented
  * Expandable for MCQs / adaptive learning

---

### 💬 AI Tutor (UI Ready)

* Chat UI implemented
* Messaging system working (UI level)
* AI integration temporarily disabled (API issues)
* Ready for:

  * Gemini / HuggingFace / local models

---

## ⚙️ Tech Stack

* **Unity 2022 LTS**
* **Vuforia Engine (Image Tracking)**
* **C# (MonoBehaviour scripting)**
* **TextMeshPro (UI)**
* **Blender (3D model processing)**

---

## 📁 Project Structure

```
Assets/
 ├── Scripts/
 │    ├── AR/
 │    ├── AI/
 │    ├── Interaction/
 │    └── Data/
 ├── Models/
 ├── Prefabs/
 ├── Resources/
 └── StreamingAssets/
      └── organs.json
```

---

## 🧠 Current Scope

✅ Fully working for:

* Brain model (external + parts)

🔜 Designed to scale for:

* Heart
* Lungs
* Liver
* Full human anatomy system

---

## 🛠️ Setup Instructions

1. Clone the repo:

```
git clone https://github.com/your-username/AnatoLens.git
```

2. Open in Unity Hub:

* Use **Unity 2022 LTS**

3. Ensure:

* Vuforia is enabled
* Camera permissions granted

4. Build:

```
File → Build Settings → Android → Build APK
```

---

## 📸 Development Progress

### 🧩 Phase 1 – AR Setup

*Add screenshot here*

```
(Insert Image)
```

---

### 🧠 Phase 2 – Brain Model Integration

*Add screenshot here*

```
(Insert Image)
```

---

### 🏷️ Phase 3 – Label System

*Add screenshot here*

```
(Insert Image)
```

---

### 💬 Phase 4 – Chat UI

*Add screenshot here*

```
(Insert Image)
```

---

### 📱 Phase 5 – Mobile Testing

*Add screenshot here*

```
(Insert Image)
```

---

## ⚠️ Known Issues

* AI API currently returning errors (integration pending fix)
* Large assets not pushed to GitHub (Git LFS needed)
* Some UI scaling issues on certain devices (partially fixed)

---

## 🔮 Upcoming Features

* 🤖 Fully working AI tutor
* 🧠 More organs (modular expansion)
* 🎯 Advanced quizzes (scoring + tracking)
* 🖼️ Image-based learning panels
* 🧭 AR navigation & guided learning
* 🌐 Cloud-based content updates

---

## 🤝 Contribution

Currently a solo development project.
Open to collaboration in:

* AI integration
* UI/UX improvements
* 3D optimization

---

## 📜 License

This project is for educational purposes.
License will be updated before public release.

---

## 💡 Vision

To build an **interactive AR-powered anatomy learning platform** that replaces static textbooks with immersive, intelligent experiences.

---
