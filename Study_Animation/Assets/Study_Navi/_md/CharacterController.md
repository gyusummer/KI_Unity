# CharacterController

## 개념

`CharacterController`는 Unity에서 물리적인 Rigidbody 없이도 캐릭터의 충돌과 이동을 처리할 수 있도록 제공하는 컴포넌트입니다.  
주로 1인칭 또는 3인칭 게임에서 플레이어 캐릭터 이동에 사용되며, 점프, 중력, 경사면 등 다양한 움직임 처리에 적합합니다.

**핵심 특징**
- Rigidbody 불필요 (물리 연산 최소화)
- 슬라이딩 없음 (기본적으로 마찰 적용 없음)
- 사용자 직접 중력 및 점프 계산 필요
- 충돌 처리는 내부적으로 제공됨

## CharacterController 컴포넌트 구성

| 항목 | 설명 |
|------|------|
| Center | 캡슐의 중심 좌표 (로컬 기준) |
| Radius | 캡슐 콜라이더의 반지름 |
| Height | 캡슐 콜라이더의 높이 |
| Slope Limit | 올라갈 수 있는 경사의 최대 각도 |
| Step Offset | 작은 턱을 넘을 수 있는 최대 높이 |
| Skin Width | 충돌 계산 시 마진 오차 (일반적으로 기본값 유지) |
| Min Move Distance | 이 거리 이하의 이동은 무시 (진동 방지용) |
| Detect Collisions | 충돌 감지 여부 설정 |

## CharacterController 클래스 API

### 주요 프로퍼티

| 프로퍼티 | 설명 | 타입 |
|----------|------|------|
| isGrounded | 현재 바닥에 닿아있는지 여부 | bool |
| velocity | 최근 Move()에서 적용된 속도 (읽기 전용) | Vector3 |
| radius, height | 콜라이더의 반지름과 높이 | float |
| center | 콜라이더의 중심 | Vector3 |
| slopeLimit | 오를 수 있는 경사의 최대각도 | float |

> isGrounded는 Move()를 호출한 직후에만 업데이트됨에 주의

### 주요 함수

#### Move(Vector3 motion)

```csharp
CollisionFlags flags = controller.Move(motion);
```
- 캐릭터를 주어진 방향으로 이동시키는 함수
- 내부적으로 충돌 판정까지 처리
- 이동량에는 Time.deltaTime이 곱해져야 함

> 반환값 CollisionFlags\
CollisionFlags.Sides (옆 충돌)\
CollisionFlags.Above (위 충돌)\
CollisionFlags.Below (바닥 충돌)
>

#### SimpleMove(Vector3 speed)
```csharp
controller.SimpleMove(transform.forward * speed);
```
- Move()와 유사하나, 자동으로 중력을 적용해줌
- 내부적으로 Time.deltaTime이 곱해지므로 중복 곱하지 않음
- 충돌 정보는 반환하지 않음

### 사용 팁 
- 이동 로직은 반드시 Move()로 처리해야 isGrounded 상태가 업데이트됨
- 중력은 개발자가 직접 계산해서 Move()에 포함시켜야 함
- CharacterController는 경사면을 처리할 수 있으나, 슬라이딩 처리는 기본적으로 없음
- Rigidbody와 함께 사용하지 않도록 주의

#### 예제 코드
```csharp
void Update()
{
    float moveX = Input.GetAxis("Horizontal");
    float moveZ = Input.GetAxis("Vertical");
    Vector3 move = transform.right * moveX + transform.forward * moveZ;

    controller.Move(move * speed * Time.deltaTime);

    if (controller.isGrounded && velocity.y < 0)
        velocity.y = -2f;

    if (Input.GetButtonDown("Jump") && controller.isGrounded)
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
}
```
---

## Rigidbody 대신 CharacterController를 사용하는 이유

## 공통점

| 항목 | CharacterController | Rigidbody |
|------|---------------------|-----------|
| 충돌 처리 | 제공함 | 제공함 (Collider와 함께) |
| 중력 적용 | 직접 구현해야 함 | 자동 적용 (gravity = true) |
| 점프 처리 | 직접 구현 필요 | AddForce() 등 사용 |
| 이동 방식 | Move() 함수로 직접 위치 이동 | 물리력(AddForce, velocity)으로 이동 |
| 슬라이딩 | 없음 (기본적으로 마찰이 강함) | 기본적으로 있음 |
| 계단/턱 넘기 | Step Offset으로 자연스럽게 가능 | 설정 복잡하거나 버그 유발 가능 |

## FPS 캐릭터에는 CharacterController가 더 유리한 이유

### 1. 정밀한 이동 제어
- 입력에 즉각 반응하는 컨트롤이 가능
- Rigidbody는 미끄러짐/관성으로 인해 제어가 어려움
- CharacterController는 직접 움직이므로 반응성이 뛰어남

### 2. 슬라이딩/관성 제거
- 물리 엔진 기반 움직임은 멈출 때도 움직이려는 경향이 있음
- FPS는 정확한 정지, 에임이 중요 → CharacterController가 더 적합

### 3. 계단/턱 넘기 기능 (Step Offset)
- Rigidbody는 작고 낮은 계단에 걸릴 수 있음
- CharacterController는 Step Offset으로 자연스럽게 올라감

### 4. 안정적인 물리 처리
- FPS 캐릭터는 예측 가능한 충돌과 이동이 중요
- CharacterController는 물리 시뮬레이션에 의존하지 않아서 안정적

### 5. 중력 및 점프 로직을 직접 제어 가능
- Rigidbody는 물리 엔진 기반이라 예상치 못한 반응이 나올 수 있음
- CharacterController는 중력, 점프 로직을 스크립트에서 정밀하게 구현 가능

## 언제 Rigidbody가 더 적합한가?

- 물리 퍼즐 게임 (예: 박스 밀기)
- 중력/마찰/충돌 등 물리 반응이 중요한 상황
- 네트워크 동기화를 위해 Rigidbody 기반의 동작이 필요한 게임

## 결론 요약

| 상황 | 추천 컴포넌트 | 이유 |
|------|----------------|------|
| 1인칭 FPS 캐릭터 | CharacterController | 정밀 제어, 안정적인 이동, 슬라이딩 없음 |
| 물리 기반 상호작용 (상자 밀기 등) | Rigidbody | 충돌, 마찰, 관성 등 물리 효과 필요 |
| 혼합형 (FPS + 물리 상자 등) | 둘 모두 사용 | 캐릭터는 CharacterController, 오브젝트는 Rigidbody 사용 |

> 보통 플레이어와 관련된 물리는 엔진에 위임하는 케이스보다는 직접 정밀하게 구현하는 케이스가 많음\
> 그리고 그게 속 편함