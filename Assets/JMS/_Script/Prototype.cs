using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype : MonoBehaviour
{
    // Wave Function Collapse
    // 각 면 마다 적용될 고유 값 적어두기
    // HorizontalFace - Left, Right, Forward, Back
    // VerticalFace - Up, Down

    // HorizontalFace는 대칭 여부 판단후 이미 뒤집은건지 아닌지 판단
    // VerticalFace는 회전이 가능한지 판단후 몇도 꺽인건지 기록

    // 와야하는 면이 대칭을 해줘야 하는지 아닌지 판별
    // 모든면이 0인 공기도 필요
    // 연결이 가능한 부분은 각 면 별로 숫자로 표시 0은 0끼리 연결 가능 1은 1끼리 연결 가능
    // Symmetric(대칭)이 가능하면 숫자 뒤에 s입력 Flipped 된게 와야하면 F
    // 회전 불가면 Invariant 의 I를따서 뒤에 i를 붙임
    // 회전이 가능하면 얼마나 꺽여있는지 적어두기
}
