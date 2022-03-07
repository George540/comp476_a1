import numpy as np
import matplotlib.pyplot as plt
import math

pc = np.array([3, 6]) #current position of character
vc = np.array([2, 3]) #current velocity of character
pt = np.array([5, 4]) #current position of target (stationary)
t = 0.25 # tick time
vm = 3.6 # maximum velocity of character
am = 12.25 # maximum acceleration of character

print('Q1a:')
x = [None] * 5
y = [None] * 5
q1a_values = [np.array([0, 0])] * 5
steps = 0
while (steps < 5):
    vd = pt - pc # get velocity direction
    vd_magnitude = math.sqrt((vd[0]*vd[0]) + (vd[1]*vd[1])) # find magnitude
    vd_normalized = vd/vd_magnitude # normalize velocity direction
    v_ksv = vm * vd_normalized # kinematic seek velocity
    pc = pc + (v_ksv* t) # update position
    print(np.round(pc, 2))
    x[steps] = pc[0]
    y[steps] = pc[1]
    q1a_values[steps] = pc
    steps += 1

plt.scatter(x, y)
plt.plot(x, y)
plt.title("Q1a) AI character's Seek (Kinematic) Path towards stationary target")
plt.xlabel("Position X")
plt.xlabel("Position Y")
plt.show()


pc = np.array([3, 6])
vc = np.array([2, 3])
pt = np.array([5, 4])
t = 0.25
vm = 3.6
am = 12.25

print('Q1b:')
x = [None] * 5
y = [None] * 5
q1b_values = [np.array([0, 0])] * 5
steps = 0
while (steps < 5):
    vd = pt - pc # get velocity direction
    vd_magnitude = math.sqrt((vd[0]*vd[0]) + (vd[1]*vd[1]))  # find magnitude
    vd_normalized = vd/vd_magnitude # normalize velocity direction
    a_ssa = am * vd_normalized # steering seek acceleration
    vc = vc + (a_ssa * t) # steering seek velocity
    vc_magnitude = math.sqrt((vc[0]*vc[0]) + (vc[1]*vc[1]))  # find magnitude
    if vc_magnitude > vm: # if it surprasses maximum velocity
        vc = (vc/vc_magnitude) * vm # normalize and multiply by maximum
    pc = pc + (vc * t) # update position
    print(np.round(pc, 2))
    x[steps] = pc[0]
    y[steps] = pc[1]
    q1b_values[steps] = pc
    steps += 1

plt.scatter(x, y)
plt.plot(x, y)
plt.title("Q1b) AI character's Steer (Kinematic) Path towards stationary target")
plt.xlabel("Position X")
plt.xlabel("Position Y")
plt.show()

pc = np.array([3, 6]) #current position of character
vc = np.array([2, 3]) #current velocity of character
pt = np.array([5, 4]) #current position of target (stationary)
t = 0.25 # tick time
vm = 3.6 # maximum velocity of character
am = 12.25 # maximum acceleration of character
r_sat = 0.5 # satisfaction radius
t2t = 0.55 # time-to-target


print('Q1d:')
x = [None] * 5
y = [None] * 5
q1d_values = [np.array([0, 0])] * 5
steps = 0
while (steps < 5):
    vd = pt - pc # get velocity direction
    vd_magnitude = math.sqrt((vd[0]*vd[0]) + (vd[1]*vd[1]))  # find magnitude
    vd_normalized = vd/np.linalg.norm(vd) # normalize velocity direction
    # Blending in I and II Kinematic Arrive 
    v = 0 # set velocity to 0 if within radius of satisfaction
    if vd_magnitude > r_sat: # if distance is bigger than radius...
        v = min(vm, vd_magnitude/t2t) # slow down for arriving
    v_kav = v * vd_normalized # kinematic arrive velocity
    pc = pc + (v_kav * t) # update position
    print(np.round(pc, 2))
    x[steps] = pc[0]
    y[steps] = pc[1]
    q1d_values[steps] = pc
    steps += 1

plt.scatter(x, y)
plt.plot(x, y)
plt.title("Q1d) AI character's Arrive (Kinematic) Path towards stationary target")
plt.xlabel("Position X")
plt.xlabel("Position Y")
plt.show()


pc = np.array([3, 6]) #current position of character
vc = np.array([2, 3]) #current velocity of character
pt = np.array([5, 4]) #current position of target (stationary)
t = 0.25 # tick time
vm = 3.6 # maximum velocity of character
am = 12.25 # maximum acceleration of character
r_arr = 0.2 # radius of arrival
r_sat = 0.5 # satisfaction radius
t2t = 0.55 # time-to-target

print('Q1e:')
x = [None] * 5
y = [None] * 5
q1e_values = [np.array([0, 0])] * 5
steps = 0
while (steps < 5):
    vd = pt - pc # get velocity direction
    vd_magnitude = math.sqrt((vd[0]*vd[0]) + (vd[1]*vd[1]))  # find magnitude
    vd_normalized = vd/vd_magnitude # normalize velocity direction
    if vd_magnitude <= r_arr: # if character arrives, stop moving
        continue
    elif vd_magnitude <= r_sat: # if character is in radius, slow down
        speed_target = (vd_magnitude/r_sat) * vm # find target speed
        # find target velocity
        v_target = vd_normalized
        v_target *= speed_target

        # find acceleration
        a = v_target - vc
        a /= t2t
        # keep acceleration within maximum
        a_magnitude = math.sqrt((a[0]*a[0]) + (a[1]*a[1]))
        if np.linalg.norm(a) > am:
            a_norm = (a/np.linalg.norm(a))
            a = a_norm * am

        # find desired velocity
        at = a * t
        v = vc + at

        v_magnitude = math.sqrt((v[0]*v[0]) + (v[1]*v[1]))
        if v_magnitude > vm:
            v = (v/v_magnitude) * vm

        vc = v
        pc = pc + (vc*t) #update position
    else: # if outside of radii, continue steering seek as usual
        a = vd_normalized *am
        v = vc + (a*t)
        v_magnitude = math.sqrt((v[0]*v[0]) + (v[1]*v[1]))
        if v_magnitude > vm:
            v = (v/v_magnitude) * vm
        vc = v
        pc = pc + (vc*t)
        
    print(np.round(pc, 2))
    x[steps] = pc[0]
    y[steps] = pc[1]
    q1e_values[steps] = pc
    steps += 1

plt.scatter(x, y)
plt.plot(x, y)
plt.title("Q1e) AI character's Arrive (Steering) Path towards stationary target")
plt.xlabel("Position X")
plt.xlabel("Position Y")
plt.show()
print()
print()
