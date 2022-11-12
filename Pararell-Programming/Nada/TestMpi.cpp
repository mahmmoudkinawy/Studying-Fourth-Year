# include <cmath>
# include <ctime>
# include <iomanip>
# include <iostream>
# include <mpi.h>   
using namespace std;

int prime_number(int n, int id, int p)
{
	int i;
	int j;
	int prime;
	int total;

	total = 0;

	for (i = 2 + id; i <= n; i = i + p)
	{
		prime = 1;
		for (j = 2; j < i; j++)
		{
			if ((i % j) == 0)
			{
				prime = 0;
				break;
			}
		}
		total = total + prime;
	}
	return total;
}

void timestamp()
{
# define TIME_SIZE 40
	static char time_buffer[TIME_SIZE];
	const struct tm* tm;
	time_t now;

#pragma warning(disable : 4996) //_CRT_SECURE_NO_WARNINGS
	now = time(NULL);
	tm = localtime(&now);

	strftime(time_buffer, TIME_SIZE, "%d %B %Y %I:%M:%S %p", tm);

	printf("%s \n", time_buffer);

	return;
# undef TIME_SIZE
}

int main(int argc, char* argv[])
{
	int id;
	int ierr;
	int n;
	int n_factor;
	int n_hi;
	int n_lo;
	int p;
	int primes;
	int primes_part;
	double wtime;

	n_lo = 1;
	n_hi = 22;
	n_factor = 2;

	ierr = MPI_Init(&argc, &argv);

	if (ierr != 0)
	{
		printf("\n");
		printf("Fatal error!\n");
		exit(1);
	}

	ierr = MPI_Comm_size(MPI_COMM_WORLD, &p);

	ierr = MPI_Comm_rank(MPI_COMM_WORLD, &id);

	if (id == 0)
	{
		timestamp();
		printf("\n");
		printf("An MPI example to count the number of primes.\n\n");
		printf("The number of processes is = %d\n", p);
		printf("\n");
		printf("         N        CT          Time\n"); //C => Count of prime numbers
	}

	n = n_lo;

	while (n <= n_hi)
	{
		if (id == 0)
		{
			wtime = MPI_Wtime();
		}
		ierr = MPI_Bcast(&n, 1, MPI_INT, 0, MPI_COMM_WORLD);

		primes_part = prime_number(n, id, p);

		ierr = MPI_Reduce(&primes_part, &primes, 1, MPI_INT, MPI_SUM, 0,
			MPI_COMM_WORLD);

		if (id == 0)
		{
			wtime = MPI_Wtime() - wtime;

			clog << "  " << setw(8) << n
				<< "  " << setw(8) << primes
				<< "  " << setw(14) << wtime << "s" << "\n";
		}
		n = n * n_factor;
	}

	MPI_Finalize();

	if (id == 0)
	{
		printf("\n\n");
		printf("End of execution.\n\n");
		timestamp();
	}

	return 0;
}