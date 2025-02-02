#if defined(VERTEX) || __VERSION__ > 100 || defined(GL_FRAGMENT_PRECISION_HIGH)
    #define MY_HIGHP_OR_MEDIUMP highp
#else
    #define MY_HIGHP_OR_MEDIUMP mediump
#endif

precision mediump float;

uniform MY_HIGHP_OR_MEDIUMP vec2 foil;
uniform MY_HIGHP_OR_MEDIUMP float dissolve;
uniform MY_HIGHP_OR_MEDIUMP float time;
uniform MY_HIGHP_OR_MEDIUMP vec4 texture_details;
uniform MY_HIGHP_OR_MEDIUMP vec2 image_details;
uniform bool shadow;
uniform MY_HIGHP_OR_MEDIUMP vec4 burn_colour_1;
uniform MY_HIGHP_OR_MEDIUMP vec4 burn_colour_2;

vec4 dissolve_mask(vec4 tex, vec2 texture_coords, vec2 uv)
{
    if (dissolve < 0.001) {
        return vec4(shadow ? vec3(0.,0.,0.) : tex.xyz, shadow ? tex.a*0.3: tex.a);
    }

    float adjusted_dissolve = (dissolve*dissolve*(3.-2.*dissolve))*1.02 - 0.01;

    float t = time * 10.0 + 2003.;
    vec2 floored_uv = (floor((uv*texture_details.ba)))/max(texture_details.b, texture_details.a);
    vec2 uv_scaled_centered = (floored_uv - 0.5) * 2.3 * max(texture_details.b, texture_details.a);
    
    vec2 field_part1 = uv_scaled_centered + 50.*vec2(sin(-t / 143.6340), cos(-t / 99.4324));
    vec2 field_part2 = uv_scaled_centered + 50.*vec2(cos( t / 53.1532),  cos( t / 61.4532));
    vec2 field_part3 = uv_scaled_centered + 50.*vec2(sin(-t / 87.53218), sin(-t / 49.0000));

    float field = (1.+ (
        cos(length(field_part1) / 19.483) + sin(length(field_part2) / 33.155) * cos(field_part2.y / 15.73) +
        cos(length(field_part3) / 27.193) * sin(field_part3.x / 21.92) ))/2.;
    vec2 borders = vec2(0.2, 0.8);

    float res = (.5 + .5* cos( (adjusted_dissolve) / 82.612 + ( field + -.5 ) *3.14));
    
    if (tex.a > 0.01 && burn_colour_1.a > 0.01 && !shadow && res < adjusted_dissolve + 0.8*(0.5-abs(adjusted_dissolve-0.5)) && res > adjusted_dissolve) {
        if (!shadow && res < adjusted_dissolve + 0.5*(0.5-abs(adjusted_dissolve-0.5)) && res > adjusted_dissolve) {
            tex.rgba = burn_colour_1.rgba;
        } else if (burn_colour_2.a > 0.01) {
            tex.rgba = burn_colour_2.rgba;
        }
    }
    return vec4(shadow ? vec3(0.,0.,0.) : tex.xyz, res > adjusted_dissolve ? (shadow ? tex.a*0.3: tex.a) : .0);
}

vec3 RGBtoHSL(vec3 c) {
    float minVal = min(c.r, min(c.g, c.b));
    float maxVal = max(c.r, max(c.g, c.b));
    float delta = maxVal - minVal;
    
    float h = 0.0, s = 0.0, l = (maxVal + minVal) * 0.5;
    
    if (delta > 0.0001) {
        s = (l < 0.5) ? (delta / (maxVal + minVal)) : (delta / (2.0 - maxVal - minVal));
        if (maxVal == c.r) h = (c.g - c.b) / delta + (c.g < c.b ? 6.0 : 0.0);
        else if (maxVal == c.g) h = (c.b - c.r) / delta + 2.0;
        else h = (c.r - c.g) / delta + 4.0;
        h /= 6.0;
    }
    return vec3(h, s, l);
}

vec3 HSLtoRGB(vec3 hsl) {
    float h = hsl.x, s = hsl.y, l = hsl.z;
    float r, g, b;
    if (s == 0.0) return vec3(l);
    float q = (l < 0.5) ? (l * (1.0 + s)) : (l + s - l * s);
    float p = 2.0 * l - q;
    vec3 t = vec3(h + 1.0 / 3.0, h, h - 1.0 / 3.0);
    
    for (int i = 0; i < 3; i++) {
        if (t[i] < 0.0) t[i] += 1.0;
        if (t[i] > 1.0) t[i] -= 1.0;
        if (t[i] < 1.0 / 6.0) t[i] = p + (q - p) * 6.0 * t[i];
        else if (t[i] < 0.5) t[i] = q;
        else if (t[i] < 2.0 / 3.0) t[i] = p + (q - p) * (2.0 / 3.0 - t[i]) * 6.0;
        else t[i] = p;
    }
    return vec3(t);
}

vec4 effect(vec4 colour, Image texture, vec2 texture_coords, vec2 screen_coords) {
    vec4 tex = Texel(texture, texture_coords);
    vec3 hsl = RGBtoHSL(tex.rgb);
    
    hsl.z = 1.0 - hsl.z;  // Invert lightness
    hsl.x = -hsl.x + 0.2; // Modify hue
    
    tex.rgb = HSLtoRGB(hsl);
    return dissolve_mask(tex, texture_coords, texture_coords);
}
