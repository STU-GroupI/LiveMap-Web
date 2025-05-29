export function computeAffine(src, dst) {
    const A = [], b = [];

    for (let i = 0; i < src.length; i++) {
        const [x, y] = src[i];
        const [u, v] = dst[i];
        A.push([x, y, 1, 0, 0, 0]);
        A.push([0, 0, 0, x, y, 1]);
        b.push(u, v);
    }

    const AT = numeric.transpose(A);
    const ATA = numeric.dot(AT, A);
    const ATb = numeric.dot(AT, b);
    return numeric.solve(ATA, ATb);
}

export function applyAffine(t, x, y) {
    const [a, b, c, d, e, f] = t;
    return [
        a * x + b * y + c,
        d * x + e * y + f
    ];
}
