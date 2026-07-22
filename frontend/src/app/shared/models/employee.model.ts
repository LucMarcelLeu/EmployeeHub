export interface EmployeeSkill {
    employeeId: string;
    skillId: string;
    skillName?: string | null;
}

export interface Employee {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    entryDate?: string | null;
    exitDate?: string | null;
    departmentId?: string | null;
    department?: string | null;
    skills?: EmployeeSkill[];
}