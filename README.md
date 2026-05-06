# DarkLab

2024-1 학기 공포 게임 프로젝트입니다. Unity 기반 3D 호러 어드벤처를 목표로, 플레이어 탐색, 상호작용, 카메라 연출, 추격/이벤트 흐름을 실험한 저장소입니다.

## 프로젝트 개요

`DarkLab`은 어두운 실내 공간을 탐색하며 단서와 오브젝트를 조사하는 공포 게임 프로토타입입니다. 저장소의 실제 Unity 프로젝트는 `Threshold/` 폴더 안에 있습니다.

## 주요 구현 영역

- 1인칭 플레이어 이동과 입력 처리
- 레이캐스트 기반 오브젝트 상호작용
- 카메라 전환, 카메라 유지, 시야 방해 오브젝트 처리
- 캐릭터 상태 데이터를 `ScriptableObject`로 분리
- NPC/이벤트 연출용 스크립트 구성
- DOTween, Cinemachine, Input System, URP 기반 연출 실험

## 기술 스택

- Unity `2022.3.2f1`
- C#
- Universal Render Pipeline
- Unity Input System
- Cinemachine
- DOTween / DOTween Pro
- Unity Localization
- Post Processing

## 폴더 구조

```text
.
├── README.md
├── gitattributes.txt
└── Threshold/
    ├── Assets/
    │   ├── Scripts/
    │   ├── ScripableObjects/
    │   ├── Scenes/
    │   └── Plugins/
    ├── Packages/
    └── ProjectSettings/
```

## 실행 방법

1. Unity Hub에서 `2022.3.2f1` 버전을 설치합니다.
2. Unity Hub의 `Add project from disk`로 `Threshold/` 폴더를 선택합니다.
3. 패키지 복원이 끝날 때까지 기다립니다.
4. `Assets/Scenes` 아래의 시작 씬을 열고 Play 버튼으로 실행합니다.

## 개발 메모

이 저장소는 완성 빌드보다는 호러 게임의 핵심 상호작용과 연출을 검증하기 위한 프로토타입에 가깝습니다. 이후 정리 시에는 씬 진입점, 조작법, 엔딩 조건, 빌드 파일 링크를 README에 추가하면 포트폴리오 문서로 더 명확해집니다.
